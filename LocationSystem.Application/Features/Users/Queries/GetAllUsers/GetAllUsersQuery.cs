using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;

namespace LocationSystem.Application.Features.Users.Queries
{
    public class GetAllUsersQuery :PageRequest, IRequest<IEnumerable<UserDto>>
    {
    }
}
