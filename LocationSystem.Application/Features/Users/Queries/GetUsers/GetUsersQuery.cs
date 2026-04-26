

using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<IQueryable<UserGraphqLDto>>
    {
    }
}
