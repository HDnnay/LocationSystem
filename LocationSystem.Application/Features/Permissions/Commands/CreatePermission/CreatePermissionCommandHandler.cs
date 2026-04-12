using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Events;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.UserRolePermissions;
using Mapster;

namespace LocationSystem.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, PermissionDto>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;

        public CreatePermissionCommandHandler(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork, IEventBus eventBus)
        {
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }

        public async Task<PermissionDto> Handle(CreatePermissionCommand request)
        {
            // 检查权限名称是否已存在
            var existingPermission = await _permissionRepository.GetByNameAsync(request.PermissionDto.Name);
            if (existingPermission != null)
            {
                throw new Exception($"权限名称 {request.PermissionDto.Name} 已存在");
            }

            // 检查权限代码是否已存在
            existingPermission = await _permissionRepository.GetByCodeAsync(request.PermissionDto.Code);
            if (existingPermission != null)
            {
                throw new Exception($"权限代码 {request.PermissionDto.Code} 已存在");
            }

            // 创建权限
            var permission = new Permission(
                name: request.PermissionDto.Name,
                code: request.PermissionDto.Code,
                description: request.PermissionDto.Description,
                parentId: request.PermissionDto.ParentId
            );

            // 保存权限
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _permissionRepository.AddAsync(permission);
                await _unitOfWork.CommitAsync();

                // 发布权限变更事件，更新缓存
                await _eventBus.PublishAsync(new PermissionsChangedEvent());
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            // 返回DTO
            return permission.Adapt<PermissionDto>();
        }
    }
}