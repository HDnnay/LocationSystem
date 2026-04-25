using LocationSystem.Application.Features.Users.Queries.GetUsers;
using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Application.Utilities;
using Mapster;

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
            var result = model.Select(t => t.Adapt<UserGraphqLDto>());
            return result;
        }
        [UsePaging(typeof(Models.UserType), IncludeTotalCount = true)]
        [UseSorting]
        [UseFiltering]
        [GraphQLDescription("获取角色列表")]
        [GraphQLName("roles")]
        public async Task<IQueryable<UserGraphqLDto>> GetRoles([Service] IMediator mediator)
        {
            var query = new GetUsersQuery();
            var model = await mediator.Send(query);
            var result = model.Select(t => t.Adapt<UserGraphqLDto>());
            return result;
        }
    }
}
