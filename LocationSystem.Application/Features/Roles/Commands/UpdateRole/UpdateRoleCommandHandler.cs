using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Roles.Commands.UpdateRole;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleDto> Handle(UpdateRoleCommand request)
        {
            // 检查角色是否存在
            var role = await _roleRepository.GetRoleWithPermissionsAsync(request.RoleId);
            if (role == null)
            {
                throw new Exception($"角色不存在，ID: {request.RoleId}");
            }

            // 检查角色名称是否已被其他角色使用
            var existingRole = await _roleRepository.GetByNameAsync(request.RoleDto.Name);
            if (existingRole != null && existingRole.Id != request.RoleId)
            {
                throw new Exception($"角色名称 {request.RoleDto.Name} 已存在");
            }

            // 检查角色代码是否已被其他角色使用
            existingRole = await _roleRepository.GetByCodeAsync(request.RoleDto.Code);
            if (existingRole != null && existingRole.Id != request.RoleId)
            {
                throw new Exception($"角色代码 {request.RoleDto.Code} 已存在");
            }

            // 更新角色信息
            role.Update(request.RoleDto.Name, request.RoleDto.Code, request.RoleDto.Description);

            // 更新角色权限
            role.ClearPermissions();
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

            // 保存更新
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _roleRepository.UpdateAsync(role);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            // 返回更新后的角色
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Code = role.Code,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt,
                Permissions = role.Permissions.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToList()
            };
        }
    }
}