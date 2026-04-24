using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Queries.GetRoleByIds
{
    public class GetRoleByIdsQuery : IRequest<Dictionary<Guid, RoleGraphqLDto>>
    {
    }
}
