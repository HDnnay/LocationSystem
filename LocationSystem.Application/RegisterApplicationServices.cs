using FluentValidation;
using LocationSystem.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application
{
    public static class RegisterApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IMediator, SimpleMediator>();
            services.AddValidatorsFromAssemblyContaining<CreateDentalOfficesCommandValidator>();
            services.AddScoped<IRequestHandler<CreateDentalOfficesCommand, Guid>,
                CreateDentalOfficesCommandHandler>();
            
            services.AddScoped<IRequestHandler<GetDentalOffcesDetailQuery, DentalOfficesDetailDto>,
                GetDentalOfficesDetailQueryHandler>();
            services.AddScoped<IRequestHandler<GetDetalOfficesListQuery, List<DentalOfficesListDto>>, 
                GetDetalOfficesListQueryHandler>();
            return services;
        }
    }
}
