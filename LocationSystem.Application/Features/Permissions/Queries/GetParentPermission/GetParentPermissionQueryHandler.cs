using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Permissons;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetParentPermission
{
    public class GetParentPermissionQueryHandler(IPermissionRepository repository) : IRequestHandler<GetParentPermissionQuery, PermissionGraphqLDto>
    {
        public Task<PermissionGraphqLDto> Handle(GetParentPermissionQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
