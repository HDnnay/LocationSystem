using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Menus.Commands.AssignPermissionsToMenu
{
    public class AssignPermissionsToMenuCommandHandler : IRequestHandler<AssignPermissionsToMenuCommand>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionMenuRepository _permissionMenuRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignPermissionsToMenuCommandHandler(IMenuRepository menuRepository, IPermissionRepository permissionRepository, IPermissionMenuRepository permissionMenuRepository, IUnitOfWork unitOfWork)
        {
            _menuRepository = menuRepository;
            _permissionRepository = permissionRepository;
            _permissionMenuRepository = permissionMenuRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AssignPermissionsToMenuCommand request)
        {
            // 获取菜单
            var menu = await _menuRepository.GetByIdAsync(request.MenuId);
            if (menu == null)
            {
                throw new System.Exception("菜单不存在");
            }
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 清除现有的权限关联
                var existingPermissionMenus = await _permissionMenuRepository.GetByMenuIdAsync(request.MenuId);
                foreach (var pm in existingPermissionMenus)
                {
                    await _permissionMenuRepository.DeleteAsync(pm);
                }

                // 添加新的权限关联
                foreach (var permissionId in request.PermissionIds)
                {
                    var permission = await _permissionRepository.GetByIdAsync(permissionId);
                    if (permission != null)
                    {
                        var permissionMenu = new PermissionMenu(permission, menu);
                        await _permissionMenuRepository.AddAsync(permissionMenu);
                    }
                }

                // 保存更改
                await _unitOfWork.CommitAsync();

            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
