using LocationSystem.Application.Features.Users.Queries.GetUsers;
using LocationSystem.Application.GrapqLDTOs;
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
        public async Task<IQueryable<UserGrapqLDto>> GetUsers([Service] IMediator mediator)
        {
            var query = new GetUsersQuery();
            var model = await mediator.Send(query);
            var result = model.Select(t => t.Adapt<UserGrapqLDto>());
            return result;
        }
    }
}
