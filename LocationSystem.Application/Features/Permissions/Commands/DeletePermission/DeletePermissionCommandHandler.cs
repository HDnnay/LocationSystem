using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Permissions.Commands.DeletePermission;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Commands.DeletePermission
{
    public class DeletePermissionCommandHandler : IRequsetHandler<DeletePermissionCommand, bool>
    {
        private readonly IPermissionRepository _permissionRepository;

        public DeletePermissionCommandHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
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
            await _permissionRepository.DeleteAsync(permission);
            return true;
        }
    }
}