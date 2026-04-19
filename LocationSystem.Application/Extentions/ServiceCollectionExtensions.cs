using LocationSystem.Application.Contrats.Services;
using LocationSystem.Application.Events;
using LocationSystem.Application.Events.Handlers;
using LocationSystem.Application.ISevices;
using LocationSystem.Application.Services;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
        
        public static IServiceCollection AddSnapshotServices(this IServiceCollection services)
        {
            services.AddScoped<ISnapshotService, SnapshotService>();
            return services;
        }

        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.AddScoped<CacheClearHandler>();
            services.AddScoped<EntityDeletedEventHandler>();

            return services;
        }
        
        public static IServiceCollection AddMapsterServices(this IServiceCollection services)
        {
            // 注册 Mapster
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            
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
            
            // 注册 Mapster
            services.AddMapsterServices();
            
            return services;
        }
    }
}