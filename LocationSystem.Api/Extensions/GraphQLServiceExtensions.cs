using HotChocolate;
using LocationSystem.Api.GraphQL;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Api.Extensions
{
    public static class GraphQLServiceExtensions
    {
        public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
        {
            // 注册 Scoped 服务
            services.AddScoped<Query>();
            services.AddScoped<Mutation>();
            services.AddScoped<MenuDataLoader>();
            services.AddScoped<PermissionDataLoader>();
            services.AddScoped<UserRolesDataLoader>();
            services.AddScoped<RolePermissionsDataLoader>();
            
            // 配置 GraphQL 服务器
            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<MutationType>()
                .AddTypes(
                    // 菜单相关
                    typeof(MenuType),
                    typeof(CreateMenuCommandType),
                    typeof(UpdateMenuCommandType),
                    // 用户相关
                    typeof(UserType),
                    typeof(CreateUserCommandType),
                    typeof(UpdateUserCommandType),
                    // 角色相关
                    typeof(RoleType),
                    typeof(CreateRoleCommandType),
                    typeof(UpdateRoleCommandType),
                    // 权限相关
                    typeof(PermissionType),
                    // 通用
                    typeof(SuccessResponseType)
                );
            
            return services;
        }
    }
}