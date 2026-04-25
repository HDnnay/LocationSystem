using LocationSystem.Application.GrapqLDTOs.Permissons;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionsByIds
{
    public class GetPermissionsByIdsQuery : IRequest<Dictionary<Guid, PermissionGraphqLDto>>
    {
        public IReadOnlyList<Guid> Ids { get; set; }
    }
}