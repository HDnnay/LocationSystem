using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Menus.Commands.AssignPermissionsToMenu
{
    public class AssignPermissionsToMenuCommandHandler : IRequestHandler<AssignPermissionsToMenuCommand>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IPermissionRepository _permissionRepository;

        public AssignPermissionsToMenuCommandHandler(IMenuRepository menuRepository, IPermissionRepository permissionRepository)
        {
            _menuRepository = menuRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task Handle(AssignPermissionsToMenuCommand request)
        {
            // 获取菜单
            var menu = await _menuRepository.GetByIdAsync(request.MenuId);
            if (menu == null)
            {
                throw new System.Exception("菜单不存在");
            }

            // 清除现有的权限关联
            menu.PermissionMenus.Clear();

            // 添加新的权限关联
            foreach (var permissionId in request.PermissionIds)
            {
                var permission = await _permissionRepository.GetByIdAsync(permissionId);
                if (permission != null)
                {
                    menu.PermissionMenus.Add(new PermissionMenu(permission, menu));
                }
            }

            // 保存更改
            await _menuRepository.UpdateAsync(menu);
        }
    }
}
