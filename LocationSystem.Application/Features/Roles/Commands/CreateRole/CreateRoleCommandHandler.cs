using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Roles.Commands.CreateRole;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Roles.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequsetHandler<CreateRoleCommand, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<RoleDto> Handle(CreateRoleCommand request)
        {
            // 检查角色名称是否已存在
            var existingRole = await _roleRepository.GetByNameAsync(request.RoleDto.Name);
            if (existingRole != null)
            {
                throw new Exception($"角色名称 {request.RoleDto.Name} 已存在");
            }

            // 检查角色代码是否已存在
            existingRole = await _roleRepository.GetByCodeAsync(request.RoleDto.Code);
            if (existingRole != null)
            {
                throw new Exception($"角色代码 {request.RoleDto.Code} 已存在");
            }

            // 创建角色
            var role = new Role(
                name: request.RoleDto.Name,
                code: request.RoleDto.Code,
                description: request.RoleDto.Description
            );

            // 添加权限
            if (request.RoleDto.PermissionIds != null && request.RoleDto.PermissionIds.Count > 0)
            {
                foreach (var permissionId in request.RoleDto.PermissionIds)
                {
                    var permission = await _permissionRepository.GetByIdAsync(permissionId);
                    if (permission != null)
                    {
                        role.AddPermission(permission);
                    }
                }
            }

            // 保存角色
            await _roleRepository.AddAsync(role);

            // 返回DTO
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Code = role.Code,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt
            };
        }
    }
}