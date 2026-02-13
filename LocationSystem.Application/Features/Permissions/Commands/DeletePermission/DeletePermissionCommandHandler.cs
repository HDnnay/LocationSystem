using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Events;
using LocationSystem.Application.Features.Permissions.Commands.DeletePermission;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Commands.DeletePermission
{
    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, bool>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;

        public DeletePermissionCommandHandler(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork, IEventBus eventBus)
        {
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }


        public async Task<bool> Handle(DeletePermissionCommand request)
        {
            // 检查权限是否存在
            var permission = await _permissionRepository.GetByIdAsync(request.PermissionId);
            if (permission == null)
            {
                throw new Exception($"权限不存在，ID: {request.PermissionId}");
            }

            // 删除权限
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _permissionRepository.DeleteAsync(permission);
                await _unitOfWork.CommitAsync();
                
                // 发布权限变更事件，更新缓存
                await _eventBus.PublishAsync(new PermissionsChangedEvent());
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            return true;
        }
    }
}