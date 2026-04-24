

using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.UserRolePermissions;

namespace LocationSystem.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<IQueryable<User>>
    {
    }
}
