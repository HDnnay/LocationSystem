using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Permissions;
using LocationSystem.Application.Dtos.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Queries.GetRoleDetail
{
    public class GetRoleDetailQueryHandler : IRequestHandler<GetRoleDetailQuery, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRoleDetailQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleDto> Handle(GetRoleDetailQuery request)
        {
            // 获取角色及其权限
            var role = await _roleRepository.GetRoleWithPermissionsAsync(request.RoleId);
            if (role == null)
            {
                throw new Exception($"角色不存在，ID: {request.RoleId}");
            }

            // 转换为DTO
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Code = role.Code,
                Description = role.Description,
                IsDisabled = role.IsDisabled,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt,
                Permissions = role.Permissions.Select(permission => new PermissionDto
                {
                    Id = permission.Id,
                    Name = permission.Name,
                    Code = permission.Code,
                    Description = permission.Description,
                    CreatedAt = permission.CreatedAt,
                    UpdatedAt = permission.UpdatedAt
                }).ToList()
            };
        }
    }
}