using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Queries.GetRoles
{
    public class GetRolesQuery : IRequest<IQueryable<RoleGraphqLDto>>
    {
    }
}
