using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Commands.UpdatePermission;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommandHandler : IRequsetHandler<UpdatePermissionCommand, PermissionDto>
    {
        private readonly IPermissionRepository _permissionRepository;

        public UpdatePermissionCommandHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionDto> Handle(UpdatePermissionCommand request)
        {
            // 检查权限是否存在
            var permission = await _permissionRepository.GetPermissionWithRolesAsync(request.PermissionId);
            if (permission == null)
            {
                throw new Exception($"权限不存在，ID: {request.PermissionId}");
            }

            // 检查权限名称是否已被其他权限使用
            var existingPermission = await _permissionRepository.GetByNameAsync(request.PermissionDto.Name);
            if (existingPermission != null && existingPermission.Id != request.PermissionId)
            {
                throw new Exception($"权限名称 {request.PermissionDto.Name} 已存在");
            }

            // 检查权限代码是否已被其他权限使用
            existingPermission = await _permissionRepository.GetByCodeAsync(request.PermissionDto.Code);
            if (existingPermission != null && existingPermission.Id != request.PermissionId)
            {
                throw new Exception($"权限代码 {request.PermissionDto.Code} 已存在");
            }

            // 更新权限信息
            permission.Update(request.PermissionDto.Name, request.PermissionDto.Code, request.PermissionDto.Description);

            // 保存更新
            await _permissionRepository.UpdateAsync(permission);

            // 返回更新后的权限
            return new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Code = permission.Code,
                Description = permission.Description,
                CreatedAt = permission.CreatedAt,
                UpdatedAt = permission.UpdatedAt,
                Roles = permission.Roles.Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Code = r.Code,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                }).ToList()
            };
        }
    }
}