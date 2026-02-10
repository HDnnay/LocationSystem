using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionList;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionList
{
    public class GetPermissionListQueryHandler : IRequsetHandler<GetPermissionListQuery, List<PermissionDto>>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionListQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<List<PermissionDto>> Handle(GetPermissionListQuery request)
        {
            // 获取所有权限及其角色
            var permissions = await _permissionRepository.GetPermissionsWithRolesAsync();

            // 转换为DTO
            return permissions.Select(permission => new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Code = permission.Code,
                Description = permission.Description,
                CreatedAt = permission.CreatedAt,
                UpdatedAt = permission.UpdatedAt,
                Roles = permission.Roles.Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Code = role.Code,
                    Description = role.Description,
                    CreatedAt = role.CreatedAt,
                    UpdatedAt = role.UpdatedAt
                }).ToList()
            }).ToList();
        }
    }
}