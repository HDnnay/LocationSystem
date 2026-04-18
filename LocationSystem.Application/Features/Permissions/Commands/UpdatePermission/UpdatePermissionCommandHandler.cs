using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos.Permissions;
using LocationSystem.Application.Dtos.Roles;
using LocationSystem.Application.Events;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, PermissionDto>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;

        public UpdatePermissionCommandHandler(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork, IEventBus eventBus)
        {
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }

        public async Task<PermissionDto> Handle(UpdatePermissionCommand request)
        {
            var permission = await _permissionRepository.GetPermissionWithRolesAsync(request.PermissionId);
            if (permission == null)
            {
                throw new Exception($"权限不存在，ID: {request.PermissionId}");
            }

            var existingPermission = await _permissionRepository.GetByNameAsync(request.PermissionDto.Name);
            if (existingPermission != null && existingPermission.Id != request.PermissionId)
            {
                throw new Exception($"权限名称 {request.PermissionDto.Name} 已存在");
            }

            existingPermission = await _permissionRepository.GetByCodeAsync(request.PermissionDto.Code);
            if (existingPermission != null && existingPermission.Id != request.PermissionId)
            {
                throw new Exception($"权限代码 {request.PermissionDto.Code} 已存在");
            }

            permission.Update(request.PermissionDto.Name, request.PermissionDto.Code, request.PermissionDto.Description, request.PermissionDto.ParentId);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _permissionRepository.UpdateAsync(permission);
                await _unitOfWork.CommitAsync();

                await _eventBus.PublishAsync(new PermissionsChangedEvent());
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return permission.Adapt<PermissionDto>();
        }
    }
}