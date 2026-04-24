using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Queries.GetRolesByUser
{
    public class GetRolesByUserQuery : IRequest<ILookup<Guid, RoleGraphqLDto>>
    {
        public IReadOnlyList<Guid> Ids { get; set; }
    }
}
