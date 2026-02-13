using FluentValidation;
using LocationSystem.Application.Contrats.Services;
using LocationSystem.Application.Events;
using LocationSystem.Application.Events.Handlers;
using LocationSystem.Application.Features.Auth.Register;
using LocationSystem.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Commands.DeleteDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Commands.UpdateDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Application.Services;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Jwt;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LocationSystem.Application
{
    public static class RegisterApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IMediator, SimpleMediator>();
            
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.Scan(s => s.FromAssembliesOf(typeof(RegisterApplicationServices))
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<>))).AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c=>c.AssignableToAny(typeof(IRequestHandler<,>))).AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c=>c.AssignableToAny(typeof(ICacheService))).AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c=>c.AssignableToAny(typeof(IJwtService))).AsImplementedInterfaces().WithSingletonLifetime()
            .AddClasses(c=>c.AssignableToAny(typeof(IUserRegistrationStrategy))).AsImplementedInterfaces().WithTransientLifetime()
            .AddClasses(c=>c.AssignableToAny(typeof(ICacheManagerService))).AsImplementedInterfaces().WithScopedLifetime()
            );
            
            // 注册事件总线
            services.AddSingleton<IEventBus, InMemoryEventBus>();
            
            // 注册事件处理程序
            services.AddScoped<CacheClearHandler>();
            
            // 注册UserRegistrationStrategyFactory
            services.AddTransient<UserRegistrationStrategyFactory>();
            
            // 注册RoleManagement
            services.AddTransient<LocationSystem.Application.Services.RoleManagement>();
            
            // 注册PermissionManagement
            services.AddTransient<LocationSystem.Application.Services.PermissionManagement>();
            
            return services;
        }
    }
}
