# 代码设计问题分析与改进方案

## 一、当前架构概述

```
┌─────────────────────────────────────────────────────────────────┐
│                        API 进程                                 │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │            EventSubscriptionService (Singleton)          │   │
│  │                                                         │   │
│  │  Startup:                                               │   │
│  │    - Create Scope                                       │   │
│  │    - Get IEventBus (Singleton)                         │   │
│  │    - Get EntityDeletedEventHandler (Scoped) ❌         │   │
│  │    - Register handler to EventBus                       │   │
│  │    - Scope Disposed                                     │   │
│  └─────────────────────────────────────────────────────────┘   │
│                            │                                    │
│                            ▼                                    │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │              InMemoryEventBus (Singleton)                │   │
│  │                                                         │   │
│  │    Handlers:                                            │   │
│  │      - EntityDeletedEventHandler (Invalid Scoped) ❌    │   │
│  │      - CacheClearHandler (Invalid Scoped) ❌            │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## 二、生命周期分析

| 组件 | 生命周期 | 问题 |
|------|----------|------|
| `IEventBus` (InMemoryEventBus) | Singleton | - |
| `EventSubscriptionService` | Singleton (BackgroundService) | - |
| `EntityDeletedEventHandler` | Scoped | ❌ 被 Singleton 引用 |
| `CacheClearHandler` | Scoped | ❌ 被 Singleton 引用 |
| `IDeletedSnapshotRepository` | Scoped | ❌ Handler 依赖已失效 |
| `IUnitOfWork` | Scoped | ❌ Handler 依赖已失效 |

## 三、核心问题

### 问题 1：Scoped Handler 被 Singleton 引用

```csharp
// EventSubscriptionService.cs (启动时执行一次)
using (var scope = _serviceProvider.CreateScope())
{
    var entityDeletedHandler = scope.ServiceProvider.GetRequiredService<EntityDeletedEventHandler>();
    eventBus.Subscribe<EntityDeletedEvent>(entityDeletedHandler.Handle); // ❌
}
// scope 结束，handler 的依赖服务已失效
```

**后果**：
- 当事件触发时，调用已失效的 Handler
- `IDeletedSnapshotRepository`、`IUnitOfWork` 等依赖已不在有效 Scope 中
- 可能导致 NullReferenceException 或 "Scope already disposed" 错误

### 问题 2：InMemoryEventBus 的 Handler 存储

```csharp
// InMemoryEventBus.cs
private readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();
```

Handler 被存储为 `Delegate`，类型信息丢失，无法在调用时动态创建正确的 Scope。

## 四、改进方案

### 方案一：事件处理时动态创建 Scope（推荐）

**核心思路**：不在启动时获取 Handler，而是在事件触发时动态创建 Scope 获取 Handler。

#### 4.1 修改 EventSubscriptionService

```csharp
修改前：
public class EventSubscriptionService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public EventSubscriptionService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 等待应用启动完成
        await Task.Delay(5000, stoppingToken);

        try
        {
            Console.WriteLine("开始订阅事件...");
            using (var scope = _serviceProvider.CreateScope())
            {
                var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                var cacheClearHandler = scope.ServiceProvider.GetRequiredService<CacheClearHandler>();
                var entityDeletedHandler = scope.ServiceProvider.GetRequiredService<EntityDeletedEventHandler>();

                // 订阅角色权限变更事件
                eventBus.Subscribe<RolePermissionsChangedEvent>(cacheClearHandler.Handle);
                Console.WriteLine("已订阅角色权限变更事件");
                // 订阅用户角色变更事件
                eventBus.Subscribe<UserRolesChangedEvent>(cacheClearHandler.Handle);
                Console.WriteLine("已订阅用户角色变更事件");
                // 订阅权限变更事件
                eventBus.Subscribe<PermissionsChangedEvent>(cacheClearHandler.Handle);
                Console.WriteLine("已订阅权限变更事件");
                // 订阅实体删除事件
                eventBus.Subscribe<EntityDeletedEvent>(entityDeletedHandler.Handle);
                Console.WriteLine("已订阅实体删除事件");
            }

            Console.WriteLine("事件订阅完成");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"事件订阅失败: {ex.Message}");
        }
    }
}
修改后：
public class EventSubscriptionService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventBus _eventBus;

    public EventSubscriptionService(IServiceProvider serviceProvider, IEventBus eventBus)
    {
        _serviceProvider = serviceProvider;
        _eventBus = eventBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(5000, stoppingToken);

        try
        {
            Console.WriteLine("开始订阅事件...");

            // 订阅实体删除事件 - 动态创建 Scope
            _eventBus.Subscribe<EntityDeletedEvent>(async (@event) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<EntityDeletedEventHandler>();
                await handler.Handle(@event);
            });
            Console.WriteLine("已订阅实体删除事件");

            Console.WriteLine("事件订阅完成");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"事件订阅失败: {ex.Message}");
        }
    }
}
```

#### 4.2 修改 EntityDeletedEventHandler

移除 `IServiceScopeFactory`，直接通过构造函数注入：

```csharp
public class EntityDeletedEventHandler
{
    private readonly IDeletedSnapshotRepository _snapshotRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EntityDeletedEventHandler> _logger;

    public EntityDeletedEventHandler(
        IDeletedSnapshotRepository snapshotRepository,
        IUnitOfWork unitOfWork,
        ILogger<EntityDeletedEventHandler> logger)
    {
        _snapshotRepository = snapshotRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(EntityDeletedEvent @event)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var snapshot = new DeletedSnapshot
            {
                EntityType = @event.EntityType,
                AssemblyQualifiedTypeName = @event.AssemblyQualifiedTypeName,
                EntityId = @event.EntityId.ToString(),
                SnapshotDataJson = @event.EntityJson,
                DeletedAt = @event.DeletedAt,
                DeletedBy = @event.DeletedBy,
                DeleteReason = @event.DeleteReason
            };

            await _snapshotRepository.AddAsync(snapshot);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("实体删除快照创建成功: {EntityType}, {EntityId}", @event.EntityType, @event.EntityId);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(e, "创建删除快照失败: {EntityType}, {EntityId}", @event.EntityType, @event.EntityId);
        }
    }
}
```

### 方案二：引入 IServiceProvider 参数

在 Handler 中注入 `IServiceProvider`，在处理事件时动态获取依赖。

```csharp
public class EntityDeletedEventHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EntityDeletedEventHandler> _logger;

    public EntityDeletedEventHandler(IServiceProvider serviceProvider, ILogger<EntityDeletedEventHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Handle(EntityDeletedEvent @event)
    {
        using var scope = _serviceProvider.CreateScope();
        var snapshotRepository = scope.ServiceProvider.GetRequiredService<IDeletedSnapshotRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        // ... 处理逻辑
    }
}
```

## 五、前后对比

### 5.1 事件订阅方式对比

| 项目 | 改进前 | 改进后 |
|------|--------|--------|
| 获取 Handler 时机 | 启动时（启动后失效） | 事件触发时 |
| Scope 创建 | 启动时创建，事件处理时已失效 | 事件处理时动态创建 |
| Handler 生命周期 | 被 Singleton 引用而失效 | 每次处理都是新实例 |
| 服务依赖有效性 | ❌ 无效（已释放） | ✅ 有效 |

### 5.2 Handler 依赖注入对比

| 项目 | 改进前 | 改进后 |
|------|--------|--------|
| IServiceScopeFactory | 存在（多余） | 不需要 |
| IDeletedSnapshotRepository | 通过 Scope 获取 | 构造函数直接注入 |
| IUnitOfWork | 通过 Scope 获取 | 构造函数直接注入 |
| 代码简洁性 | 复杂 | 简洁 |

### 5.3 架构图对比

**改进前**：
```
Singleton EventBus → 持有已失效的 Scoped Handler → ❌ 错误
```

**改进后**：
```
Singleton EventBus → 收到事件 → 动态创建 Scope → 获取有效 Handler → ✅ 正确
```

## 六、完整改进流程图

```
┌─────────────────────────────────────────────────────────────────┐
│                      改进后的架构                               │
│                                                                 │
│  1. 启动阶段                                                    │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  EventSubscriptionService (Singleton)                    │   │
│  │    - 注入 IServiceProvider                              │   │
│  │    - 注入 IEventBus (Singleton)                         │   │
│  │    - 订阅事件，传入 Lambda 表达式                        │   │
│  │      (Lambda 保存了对 IServiceProvider 的引用)          │   │
│  └─────────────────────────────────────────────────────────┘   │
│                            │                                    │
│  2. 事件触发阶段                                                │
│                            ▼                                    │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  EventBus.PublishAsync(EntityDeletedEvent)              │   │
│  │    → 查找订阅的 Lambda                                   │   │
│  │    → 执行 Lambda:                                        │   │
│  │      using var scope = CreateScope()  ← 动态创建        │   │
│  │      var handler = scope.Get<Handler>()  ← 新实例      │   │
│  │      await handler.Handle(@event)                        │   │
│  │      scope.Dispose()                                     │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## 七、注意事项

1. **不只在启动时订阅**：Lambda 表达式捕获了 `IServiceProvider`，在事件触发时可随时创建 Scope
2. **每次事件创建新 Scope**：确保 Handler 获得有效的依赖实例
3. **保持 Handler 无状态**：由于每次创建新实例，Handler 应设计为无状态
4. **统一处理其他 Handler**：同样需要检查 `CacheClearHandler` 等其他 Handler 是否有相同问题
