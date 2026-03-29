using HotChocolate;
using HotChocolate.Data;
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
              services.AddScoped<UserRolesDataLoader>();
              services.AddScoped<RolePermissionsDataLoader>();
              services.AddScoped<IArticleRepository, ArticleRepository>();
              services.AddScoped<ITagRepository, TagRepository>();
            
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
                      // 文章相关
                      typeof(ArticleType),
                      typeof(TagType),
                      typeof(ArticleCommentType),
                      // 通用
                      typeof(SuccessResponseType)
                  );
              // 注意：HotChocolate.Data 的 AddPaging、AddSorting 和 AddFiltering 方法可能在当前版本中不可用
              // 我们已经在 Query 类中使用了 [UsePaging]、[UseSorting] 和 [UseFiltering] 特性
            
            return services;
        }
    }
}