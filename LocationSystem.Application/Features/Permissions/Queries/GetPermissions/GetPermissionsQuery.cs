using LocationSystem.Application.GrapqLDTOs.Permissons;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissions
{
    public class GetPermissionsQuery : IRequest<IQueryable<PermissionGraphqLDto>>
    {
    }
}
