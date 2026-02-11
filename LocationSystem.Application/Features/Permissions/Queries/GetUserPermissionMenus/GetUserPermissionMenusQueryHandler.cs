using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Application.Features.Permissions.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetUserPermissionMenus
{
    public class GetUserPermissionMenusQueryHandler : IRequestHandler<GetUserPermissionMenusQuery, List<PermissionMenuDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMenuRepository _menuRepository;

        public GetUserPermissionMenusQueryHandler(IUserRepository userRepository, IMenuRepository menuRepository)
        {
            _userRepository = userRepository;
            _menuRepository = menuRepository;
        }

        public async Task<List<PermissionMenuDto>> Handle(GetUserPermissionMenusQuery request)
        {
            // 获取用户及其角色和权限
            var user = await _userRepository.GetUserWithRolesAndPermissionsAsync(request.UserId);
            if (user == null)
            {
                return new List<PermissionMenuDto>();
            }

            // 获取用户角色的所有权限ID
            var userPermissionIds = new List<Guid>();
            foreach (var role in user.Roles)
            {
                foreach (var permission in role.Permissions)
                {
                    if (!userPermissionIds.Contains(permission.Id))
                    {
                        userPermissionIds.Add(permission.Id);
                    }
                }
            }

            // 获取与这些权限关联的所有菜单，包含权限信息
            var userMenus = await _menuRepository.GetMenusByPermissionIdsAsync(userPermissionIds);

            // 转换为DTO
            var menuDtos = userMenus
                .Select(m => {
                    // 获取菜单关联的第一个权限代码作为菜单的权限标识
                    var permissionCode = m.PermissionMenus
                        .FirstOrDefault(pm => userPermissionIds.Contains(pm.PermissionId))?
                        .Permission?.Code;
                    
                    return new PermissionMenuDto
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Code = permissionCode,
                        Description = null,
                        ParentId = m.ParentId,
                        IsMenu = true,
                        MenuPath = m.Path,
                        MenuIcon = m.Icon,
                        Order = m.Order,
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt ?? m.CreatedAt
                    };
                })
                .ToList();

            // 构造树形菜单
            return BuildMenuTree(menuDtos);
        }

        // 递归构造菜单树
        private List<PermissionMenuDto> BuildMenuTree(List<PermissionMenuDto> menuDtos)
        {
            // 创建菜单映射，便于快速查找
            var menuMap = menuDtos.ToDictionary(m => m.Id);
            var rootMenus = new List<PermissionMenuDto>();

            // 遍历所有菜单，构建树形结构
            foreach (var menu in menuDtos)
            {
                if (menu.ParentId == null)
                {
                    // 没有父菜单的菜单作为根菜单
                    rootMenus.Add(menu);
                }
                else if (menuMap.TryGetValue(menu.ParentId.Value, out var parentMenu))
                {
                    // 有父菜单的菜单添加到父菜单的子菜单列表中
                    parentMenu.Children.Add(menu);
                }
            }

            // 按Order排序
            SortMenusByOrder(rootMenus);

            return rootMenus;
        }

        // 递归按Order排序菜单
        private void SortMenusByOrder(List<PermissionMenuDto> menus)
        {
            // 按Order排序当前菜单列表
            menus.Sort((a, b) => a.Order.CompareTo(b.Order));

            // 递归排序子菜单
            foreach (var menu in menus)
            {
                SortMenusByOrder(menu.Children);
            }
        }
    }
}
