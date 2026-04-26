using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Permissons;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissions
{
    public class GetPermissionsQueryHandler(IPermissionRepository repository) : IRequestHandler<GetPermissionsQuery, IQueryable<PermissionGraphqLDto>>
    {
        public async Task<IQueryable<PermissionGraphqLDto>> Handle(GetPermissionsQuery request)
        {
            var result = repository.Query().ProjectToType<PermissionGraphqLDto>();
            return await Task.FromResult(result);
        }
    }
}
