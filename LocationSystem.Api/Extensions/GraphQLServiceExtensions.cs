using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Pagination;
using HotChocolate.Data.Sorting;
using LocationSystem.Api.GraphQL;
using LocationSystem.Api.GraphQL.Commands;
using LocationSystem.Api.GraphQL.DataLoaders;
using LocationSystem.Api.GraphQL.Types;
using LocationSystem.Application.Contrats;
using LocationSystem.Infrastructure.Repositories;
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
              services.AddScoped<UserDataLoader>();
              services.AddScoped<UserRolesDataLoader>();
              services.AddScoped<RoleDataLoader>();
              services.AddScoped<RolePermissionsDataLoader>();
              // 文章相关 DataLoader
              services.AddScoped<ArticleDataLoader>();
              services.AddScoped<ArticleTagsDataLoader>();
              services.AddScoped<ArticleCommentsDataLoader>();
              services.AddScoped<ArticleCreateUserDataLoader>();
            
            // 配置 GraphQL 服务器
              services
                  .AddGraphQLServer()
                  .AddQueryType<Query>()
                  .AddMutationType<Mutation>()
                  .AddSorting()
                  .AddFiltering()
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
                      // 文章相关
                      typeof(ArticleType),
                      typeof(TagType),
                      typeof(ArticleCommentType)
                  );
            
            return services;
        }
    }
}