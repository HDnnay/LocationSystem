using FluentValidation;
using LocationSystem.Application.Extentions;
using LocationSystem.Application.Security;
using LocationSystem.Application.Services;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Jwt;
using LocationSystem.Core.Security.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LocationSystem.Application
{
    public static class RegisterApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMediator, SimpleMediator>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.Scan(s => s.FromAssembliesOf(typeof(RegisterApplicationServices))
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<>))).AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableToAny(typeof(IRequestHandler<,>))).AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableToAny(typeof(ICacheService))).AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableToAny(typeof(IJwtService))).AsImplementedInterfaces().WithSingletonLifetime()
            );

            // 注册事件总线和缓存服务
            services.AddEventBusAndCacheServices();

            // 注册快照服务
            services.AddSnapshotServices();

            // 注册RoleManagement
            services.AddTransient<RoleManagement>();

            // 注册PermissionManagement
            services.AddTransient<PermissionManagement>();

            // 注册 Mapster
            services.AddMapsterServices();
            // 注册基础权限提供器
            services.AddScoped<IPermissionProvider, PermissionProvider>();
            // 注册权限验证器
            services.AddScoped<IPermissionValidator, PermissionValidator>();
            return services;
        }
    }
}
