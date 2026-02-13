using LocationSystem.Application.Contrats.Services;
using LocationSystem.Application.Events;
using LocationSystem.Application.Events.Handlers;
using LocationSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBusServices(this IServiceCollection services)
        {
            // 注册事件总线
            services.AddSingleton<IEventBus, InMemoryEventBus>();
            
            return services;
        }
        
        public static IServiceCollection AddCacheManagerServices(this IServiceCollection services)
        {
            // 注册缓存管理服务
            services.AddScoped<ICacheManagerService, CacheManagerService>();
            
            return services;
        }
        
        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            // 注册事件处理程序
            services.AddScoped<CacheClearHandler>();
            
            return services;
        }
        
        public static IServiceCollection AddEventBusAndCacheServices(this IServiceCollection services)
        {
            // 注册事件总线服务
            services.AddEventBusServices();
            
            // 注册缓存管理服务
            services.AddCacheManagerServices();
            
            // 注册事件处理程序
            services.AddEventHandlers();
            
            return services;
        }
    }
}