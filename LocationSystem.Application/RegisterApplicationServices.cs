using FluentValidation;
using LocationSystem.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Commands.DeleteDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Commands.UpdateDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Application.Utilities;
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
            );
            //services.AddScoped<IRequestHandler<CreateDentalOfficesCommand, Guid>,
            //    CreateDentalOfficesCommandHandler>();
            
            //services.AddScoped<IRequestHandler<GetDentalOffcesDetailQuery, DentalOfficesDetailDto>,
            //    GetDentalOfficesDetailQueryHandler>();
            //services.AddScoped<IRequestHandler<GetDetalOfficesListQuery, List<DentalOfficesListDto>>, 
            //    GetDetalOfficesListQueryHandler>();
            //services.AddScoped<IRequestHandler<UpdateDetalOfficeCommand>, UpdateDnetalOfficeCommandHandler>();
            //services.AddScoped<IRequestHandler<DeleteDentalOfficeCommand>, DeleteDentalOfficeCommandHandler>();
            return services;
        }
    }
}
