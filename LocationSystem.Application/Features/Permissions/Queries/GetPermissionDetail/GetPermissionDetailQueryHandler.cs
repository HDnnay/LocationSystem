using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionDetail;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionDetail
{
    public class GetPermissionDetailQueryHandler : IRequsetHandler<GetPermissionDetailQuery, PermissionDto>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionDetailQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionDto> Handle(GetPermissionDetailQuery request)
        {
            // 获取权限及其角色
            var permission = await _permissionRepository.GetPermissionWithRolesAsync(request.PermissionId);
            if (permission == null)
            {
                throw new Exception($"权限不存在，ID: {request.PermissionId}");
            }

            // 转换为DTO
            return new PermissionDto
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
            };
        }
    }
}