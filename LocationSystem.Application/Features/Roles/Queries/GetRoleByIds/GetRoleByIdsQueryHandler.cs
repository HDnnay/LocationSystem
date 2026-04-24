using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Queries.GetRoleByIds
{
    public class GetRoleByIdsQueryHandler : IRequestHandler<GetRoleByIdsQuery, Dictionary<Guid, RoleGraphqLDto>>
    {
        private readonly IRoleRepository _repository;
        public GetRoleByIdsQueryHandler(IRoleRepository roleRepository)
        {
            _repository  = roleRepository;
        }
        public Task<Dictionary<Guid, RoleGraphqLDto>> Handle(GetRoleByIdsQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
