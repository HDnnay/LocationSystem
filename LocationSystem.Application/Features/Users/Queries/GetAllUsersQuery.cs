using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}
