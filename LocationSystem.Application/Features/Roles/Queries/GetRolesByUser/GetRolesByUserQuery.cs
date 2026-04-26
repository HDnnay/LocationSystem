using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Queries.GetRolesByUser
{
    public class GetRolesByUserQuery : IRequest<Dictionary<Guid, List<RoleGraphqLDto>>>
    {
        public IReadOnlyList<Guid> Ids { get; set; }
    }
}
