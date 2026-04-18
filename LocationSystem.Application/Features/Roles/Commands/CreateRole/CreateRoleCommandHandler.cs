using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos.Roles;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.UserRolePermissions;
using Mapster;

namespace LocationSystem.Application.Features.Roles.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateRoleCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleDto> Handle(CreateRoleCommand request)
        {
            // 检查角色名称是否已存在
            var existingRole = await _roleRepository.GetByNameAsync(request.Name);
            if (existingRole != null)
            {
                throw new Exception($"角色名称 {request.Name} 已存在");
            }

            // 检查角色代码是否已存在
            existingRole = await _roleRepository.GetByCodeAsync(request.Code);
            if (existingRole != null)
            {
                throw new Exception($"角色代码 {request.Code} 已存在");
            }

            // 创建角色
            var role = new Role(
                name: request.Name,
                code: request.Code,
                description: request.Description
            );

            // 添加权限
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

            // 保存角色
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _roleRepository.AddAsync(role);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            // 返回DTO
            return role.Adapt<RoleDto>();
        }
    }
}