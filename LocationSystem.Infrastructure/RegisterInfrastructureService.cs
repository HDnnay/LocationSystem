using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Infrastructure.Repositories;
using LocationSystem.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Infrastructure
{
    public static class RegisterInfrastructureService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register Infrastructure services here
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("name=SqlServerConnectionString"));
            #region ²Ö´¢×¢²á
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IRentHouseRepository, RentHouseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IPermissionMenuRepository, PermissionMenuRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleTagRepository, ArticleTagRepository>();
            services.AddScoped<IArticleCommentRepository, ArticleCommentRepository>();
            services.AddScoped<IArticleImageRepository, ArticleImageRepository>();
            services.AddScoped<IArticleLogRepository, ArticleLogRepository>();
            services.AddScoped<IDeletedSnapshotRepository, DeletedSnapshotRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWorkCore>();
            #endregion 
            return services;
        }
    }
}
