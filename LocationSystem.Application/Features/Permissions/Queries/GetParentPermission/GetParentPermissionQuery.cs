using LocationSystem.Application.GrapqLDTOs.Permissons;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetParentPermission
{
    public class GetParentPermissionQuery : IRequest<PermissionGraphqLDto>
    {
        public Guid Id { get; set; }
    }
}
