using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Permissons;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionsByIds
{
    public class GetPermissionsByIdsQueryHandler : IRequestHandler<GetPermissionsByIdsQuery, Dictionary<Guid, PermissionGraphqLDto>>
    {
        private readonly IPermissionRepository _repository;

        public GetPermissionsByIdsQueryHandler(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<Guid, PermissionGraphqLDto>> Handle(GetPermissionsByIdsQuery request)
        {
            return await _repository.GetPermissionsByIdsAsync(request.Ids);
        }
    }
}