using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSystem.Application.Services
{
    public class PermissionManagement
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMenuRepository _menuRepository;

        public PermissionManagement(IUserRepository userRepository, IRoleRepository roleRepository, IMenuRepository menuRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
        }

        /// <summary>
        /// 获取用户的所有权限ID，去重
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>权限ID列表</returns>
        public async Task<List<Guid>> GetUserPermissionIdsAsync(Guid userId)
        {
            // 获取用户的所有角色
            var userRoles = await _roleRepository.GetRolesByUserIdAsync(userId);
            var isadmin = userRoles.Any(t => t.Code=="Admin");
            if (!userRoles.Any())
            {
                return default!;
            }

            // 获取所有角色的权限，去重
            var permissionIds = new HashSet<Guid>();
            foreach (var role in userRoles)
            {
                if (role != null && role.Permissions != null)
                {
                    foreach (var permission in role.Permissions)
                    {
                        if (permission != null)
                        {
                            permissionIds.Add(permission.Id);
                        }
                    }
                }
            }
            return permissionIds.ToList();
        }
        /// <summary>
        /// 获取用户有权限的菜单
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>菜单列表</returns>
        public async Task<List<Menu>> GetUserPermissionMenusAsync(Guid userId)
        {
            // 获取用户的所有权限ID
            var userPermissionIds = await GetUserPermissionIdsAsync(userId);
            var userRoles = await _roleRepository.GetRolesByUserIdAsync(userId);
            var isAdmin = userRoles.Any(t => t.Code=="admin");
            if (!userPermissionIds.Any())
            {
                return new List<Menu>();
            }
            // 获取所有菜单，包含权限信息
            var allMenus = await _menuRepository.GetAllWithPermissionsAsync();
            List<Menu>? userMenus;
            if (isAdmin)
            {
                // 根据权限ID过滤菜单
                // 包含两种情况：
                // 1. 菜单有与用户权限关联的PermissionMenus
                // 2. 菜单没有PermissionMenus（即没有与任何权限关联），视为公共菜单
                userMenus = allMenus.Where(menu =>
                    menu.PermissionMenus.Any(pm => userPermissionIds.Contains(pm.PermissionId)) ||
                    !menu.PermissionMenus.Any()
                ).ToList();
            }
            else
            {
                userMenus=allMenus.Where(menu =>
                    menu.PermissionMenus.Any(pm => userPermissionIds.Contains(pm.PermissionId))
                ).ToList();
            }
            // 确保包含所有父菜单
            var menuIds = new HashSet<Guid>(userMenus.Select(m => m.Id));
            var allMenuDict = allMenus.ToDictionary(m => m.Id);

            // 遍历所有用户有权限的菜单，确保其父菜单也被包含
            foreach (var menu in userMenus)
            {
                var currentMenu = menu;
                while (currentMenu.ParentId.HasValue && allMenuDict.ContainsKey(currentMenu.ParentId.Value))
                {
                    var parentMenu = allMenuDict[currentMenu.ParentId.Value];
                    if (!menuIds.Contains(parentMenu.Id))
                    {
                        menuIds.Add(parentMenu.Id);
                    }
                    currentMenu = parentMenu;
                }
            }

            // 根据menuIds获取最终的菜单列表
            var finalMenus = allMenus.Where(m => menuIds.Contains(m.Id)).ToList();

            return finalMenus;
        }

        /// <summary>
        /// 检查用户是否有指定权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionCode">权限代码</param>
        /// <returns>是否有权限</returns>
        public async Task<bool> HasPermissionAsync(Guid userId, string permissionCode)
        {
            // 获取用户的所有角色
            var userRoles = await _roleRepository.GetRolesByUserIdAsync(userId);
            if (!userRoles.Any())
            {
                return false;
            }

            // 检查用户的角色是否包含指定权限
            foreach (var role in userRoles)
            {
                if (role != null && role.Permissions != null)
                {
                    foreach (var permission in role.Permissions)
                    {
                        if (permission != null && permission.Code == permissionCode)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}