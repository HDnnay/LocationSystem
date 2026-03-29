using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Events;
using LocationSystem.Application.Features.Roles.Commands.UpdateRole;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using AutoMapper;
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
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository, IUnitOfWork unitOfWork, IEventBus eventBus, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
            _mapper = mapper;
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
            var existingRole = await _roleRepository.GetByNameAsync(request.Name);
            if (existingRole != null && existingRole.Id != request.RoleId)
            {
                throw new Exception($"角色名称 {request.Name} 已存在");
            }

            // 检查角色代码是否已被其他角色使用
            existingRole = await _roleRepository.GetByCodeAsync(request.Code);
            if (existingRole != null && existingRole.Id != request.RoleId)
            {
                throw new Exception($"角色代码 {request.Code} 已存在");
            }

            // 更新角色信息
            role.Update(request.Name, request.Code, request.Description);

            // 更新角色权限
            role.ClearPermissions();
            if (request.PermissionIds != null && request.PermissionIds.Count > 0)
            {
                foreach (var permissionId in request.PermissionIds)
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
                
                // 发布角色权限变更事件，更新缓存
                await _eventBus.PublishAsync(new RolePermissionsChangedEvent { RoleId = role.Id });
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            // 返回更新后的角色
            return _mapper.Map<RoleDto>(role);
        }
    }
}