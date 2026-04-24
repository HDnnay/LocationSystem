using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Queries.GetRoleByIds
{
    public class GetRoleByIdsQuery : IRequest<Dictionary<Guid, RoleGraphqLDto>>
    {
        public GetRoleByIdsQuery(IReadOnlyList<Guid> ids)
        {
            Ids = ids;
        }
        public IReadOnlyList<Guid>? Ids { get; set; }

    }
}
