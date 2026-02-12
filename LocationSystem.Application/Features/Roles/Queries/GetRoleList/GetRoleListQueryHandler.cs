using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Roles.Queries.GetRoleList;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return roles.Select(role => new RoleDto
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
            }).ToList();
        }
    }
}