using LocationSystem.Application.Features.Articles.Queries.GetArticles;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissions;
using LocationSystem.Application.Features.Roles.Queries.GetRoles;
using LocationSystem.Application.Features.Users.Queries.GetUsers;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.GrapqLDTOs.Permissons;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.GraphQL
{
    public class Query
    {
        [UsePaging(typeof(Models.UserType), IncludeTotalCount = true)]
        [UseSorting]
        [UseFiltering]
        [GraphQLDescription("获取用户列表")]
        [GraphQLName("users")]
        public async Task<IQueryable<UserGraphqLDto>> GetUsers([Service] IMediator mediator)
        {
            var query = new GetUsersQuery();
            var model = await mediator.Send(query);
            return model;
        }
        [UsePaging(typeof(Models.RoleType), IncludeTotalCount = true)]
        [UseSorting]
        [UseFiltering]
        [GraphQLDescription("获取角色列表")]
        [GraphQLName("roles")]
        public async Task<IQueryable<RoleGraphqLDto>> GetRoles([Service] IMediator mediator)
        {
            var query = new GetRolesQuery();
            var model = await mediator.Send(query);
            return model;
        }
        [UsePaging(typeof(Models.PermissionType), IncludeTotalCount = true)]
        [UseSorting]
        [UseFiltering]
        [GraphQLDescription("获取权限列表")]
        [GraphQLName("permissions")]
        public async Task<IQueryable<PermissionGraphqLDto>> GetPermissions([Service] IMediator mediator)
        {
            var query = new GetPermissionsQuery();
            var model = await mediator.Send(query);
            return model;
        }

        [UsePaging(typeof(Models.MenuType), IncludeTotalCount = true)]
        [UseSorting]
        [UseFiltering]
        [GraphQLDescription("获取菜单列表")]
        [GraphQLName("menus")]
        public async Task<IQueryable<PermissionGraphqLDto>> GetMenus([Service] IMediator mediator)
        {
            var query = new GetPermissionsQuery();
            var model = await mediator.Send(query);
            return model;
        }

        [UsePaging(typeof(Models.ArticleType), IncludeTotalCount = true)]
        [UseSorting]
        [UseFiltering]
        [GraphQLDescription("获取菜单列表")]
        [GraphQLName("articles")]
        public async Task<IQueryable<ArticleGraphqLDto>> GetArticless([Service] IMediator mediator)
        {
            var query = new GetArticlesQuery();
            var model = await mediator.Send(query);
            return model;
        }
    }
}
