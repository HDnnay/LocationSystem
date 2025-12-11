using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Infrastructure.Repositories;
using LocationSystem.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure
{
    public static class RegisterInfrastructureService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register Infrastructure services here
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("name=SqlServerConnectionString"));
            services.AddScoped<IDentalOfficeRepositoty,DentalOfficeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWorkCore>();
            return services;
        }
    }
}
