using LocationSystem.Application.Events;
using LocationSystem.Application.Events.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocationSystem.Api.BackgroudServices
{
    public class EventSubscriptionService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EventSubscriptionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 等待应用启动完成
            Task.Delay(5000, stoppingToken).Wait();

            try
            {
                Console.WriteLine("开始订阅事件...");
                using (var scope = _serviceProvider.CreateScope())
                {
                    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                    var cacheClearHandler = scope.ServiceProvider.GetRequiredService<CacheClearHandler>();
                    // 订阅角色权限变更事件
                    eventBus.Subscribe<RolePermissionsChangedEvent>(cacheClearHandler.Handle);
                    Console.WriteLine("已订阅角色权限变更事件");
                    // 订阅用户角色变更事件
                    eventBus.Subscribe<UserRolesChangedEvent>(cacheClearHandler.Handle);
                    Console.WriteLine("已订阅用户角色变更事件");
                    // 订阅权限变更事件
                    eventBus.Subscribe<PermissionsChangedEvent>(cacheClearHandler.Handle);
                    Console.WriteLine("已订阅权限变更事件");
                }

                Console.WriteLine("事件订阅完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"事件订阅失败: {ex.Message}");
            }
            return Task.CompletedTask;
        }
    }
}