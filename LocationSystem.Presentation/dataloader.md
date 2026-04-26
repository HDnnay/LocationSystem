# DataLoader 并发问题解决方案

## 问题分析

这个错误是典型的 **DbContext 并发使用** 问题。在 HotChocolate (GraphQL) 中，解析器默认是并行执行的，多个字段的解析（包括 DataLoader 触发）会同时使用同一个 Scoped 生命周期的 DbContext 实例，导致 Entity Framework Core 抛出此异常。

## 问题场景

在你的场景中：

```csharp
using GreenDonut;
using LocationSystem.Application.Features.Users.Queries.GetUserByIds;
using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Presentation.DataLoaders
{
    public class UserDataLoader : BatchDataLoader<Guid, UserGraphqLDto>
    {
        private readonly IMediator _mediator;

        public UserDataLoader(
            IMediator mediator,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _mediator = mediator;
        }

        protected override async Task<IReadOnlyDictionary<Guid, UserGraphqLDto>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            var query = new GetUserByIdsQuery(keys);
            var model = await _mediator.Send(query);
            return model.ToDictionary(t => t.Key, t => t.Adapt<UserGraphqLDto>());
        }
    }
}
```

`UserDataLoader` 通过 `IMediator` 间接使用了 `IUserRepository`，而仓库又依赖了 `DbContext`。

当多个文章同时请求 `creator` 字段时，同一个 `DbContext` 可能在多个线程中被并行操作，即使 `LoadBatchAsync` 只执行一次查询，也可能与其他正在执行的数据库操作冲突（例如别的节点或字段也在使用 `DbContext`）。

## ✅ 解决方案：为每次批处理创建独立的 DbContext 作用域

推荐使用 `IServiceScopeFactory` 在 DataLoader 内部创建一个新的服务作用域，从而获得独立的 `DbContext` 实例，避免并发冲突。

### 修改 UserDataLoader

```csharp
public class UserDataLoader : BatchDataLoader<Guid, UserGraphqLDto>
{
    private readonly IServiceScopeFactory _scopeFactory; // 注入作用域工厂

    public UserDataLoader(
        IServiceScopeFactory scopeFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task<IReadOnlyDictionary<Guid, UserGraphqLDto>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken)
    {
        // 创建临时作用域，确保 DbContext 隔离
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var query = new GetUserByIdsQuery(keys);
        var model = await mediator.Send(query, cancellationToken);
        return model;
    }
}
```

### 确保服务注册正确

你的 `Startup.cs` / `Program.cs` 中应已含有以下注册：

```csharp
services.AddDbContext<YourDbContext>(options => ...);  // Scoped 生命周期

services.AddScoped<IUserRepository, UserRepository>();
services.AddMediatR(...);
```

`IServiceScopeFactory` 是框架默认就有的单例服务，无需额外注册。

## 📌 其他可选方案（按需选择）

### 1. 使用 IDbContextFactory<TContext> 创建短期 DbContext

如果希望彻底避免作用域问题，可以让 `IUserRepository` 不直接依赖 `DbContext`，而是依赖 `IDbContextFactory<T>`，并用 `using` 自行创建上下文。但这样会失去同一请求中事务共享等特性，通常不推荐。

### 2. 禁用 GraphQL 并行执行（不推荐）

在 `Startup` 中配置 HotChocolate：

```csharp
services.AddGraphQLServer()
    .ModifyRequestOptions(opt => opt.ExecutionTimeout = TimeSpan.FromSeconds(30))
    .ModifyOptions(opt => opt.DefaultQueryDependencyInjectionScope = ExecutionScope.Request);
```

但这样会降低吞吐量，只用在不方便修改 DataLoader 的临时方案。

## 🔍 为什么你的原始代码会报错？

即使 `LoadBatchAsync` 内部只是一个 `await _context.Users...ToDictionaryAsync()`，但同一 `DbContext` 实例可能正被另一个字段的解析器使用（例如正在查询 `comments` 字段的 `ArticleCommentDataLoader`），它们在**不同的线程**上同时操作同一个上下文实例，因此报错。

通过 `IServiceScopeFactory` 为每次批处理创建一个新的作用域，每个作用域都会获得独立的 `DbContext` 实例，从而完全规避线程安全问题。这也是 EF Core 处理 Web 应用中多线程并发的最佳实践。