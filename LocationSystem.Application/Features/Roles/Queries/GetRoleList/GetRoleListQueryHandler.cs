using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Roles;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.Roles.Queries.GetRoleList
{
    public class GetRoleListQueryHandler : IRequestHandler<GetRoleListQuery, List<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRoleListQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<RoleDto>> Handle(GetRoleListQuery request)
        {
            // 获取所有角色及其权限
            var roles = await _roleRepository.GetRolesWithPermissionsAsync();

            // 转换为DTO
            return roles.Select(role => role.Adapt<RoleDto>()).ToList();
        }
    }
}