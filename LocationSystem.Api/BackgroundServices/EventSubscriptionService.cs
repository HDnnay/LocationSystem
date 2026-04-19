using LocationSystem.Application.Events;
using LocationSystem.Application.Events.Handlers;

namespace LocationSystem.Api.BackgroudServices
{
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

                // 订阅实体删除事件 - 动态创建 Scope 获取 Handler
                _eventBus.Subscribe<EntityDeletedEvent>(async (@event) =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<EntityDeletedEventHandler>();
                    await handler.Handle(@event);
                });
                Console.WriteLine("已订阅实体删除事件");

                // 订阅角色权限变更事件 - 动态创建 Scope 获取 Handler
                _eventBus.Subscribe<RolePermissionsChangedEvent>(async (@event) =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<CacheClearHandler>();
                    await handler.Handle(@event);
                });
                Console.WriteLine("已订阅角色权限变更事件");

                // 订阅用户角色变更事件 - 动态创建 Scope 获取 Handler
                _eventBus.Subscribe<UserRolesChangedEvent>(async (@event) =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<CacheClearHandler>();
                    await handler.Handle(@event);
                });
                Console.WriteLine("已订阅用户角色变更事件");

                // 订阅权限变更事件 - 动态创建 Scope 获取 Handler
                _eventBus.Subscribe<PermissionsChangedEvent>(async (@event) =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<CacheClearHandler>();
                    await handler.Handle(@event);
                });
                Console.WriteLine("已订阅权限变更事件");

                Console.WriteLine("事件订阅完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"事件订阅失败: {ex.Message}");
            }
        }
    }
}