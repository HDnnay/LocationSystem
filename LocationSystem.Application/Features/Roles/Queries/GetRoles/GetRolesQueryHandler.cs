using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.Roles.Queries.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IQueryable<RoleGraphqLDto>>
    {
        private readonly IRoleRepository _repository;
        public GetRolesQueryHandler(IRoleRepository roleRepository)
        {
            _repository = roleRepository;
        }
        public Task<IQueryable<RoleGraphqLDto>> Handle(GetRolesQuery request)
        {
            var result = _repository.Query().ProjectToType<RoleGraphqLDto>();
            return Task.FromResult(result);
        }
    }
}
