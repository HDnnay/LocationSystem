using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Permissions.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetUserPermissionMenus
{
    public class GetUserPermissionMenusQueryHandler : IRequestHandler<GetUserPermissionMenusQuery, List<PermissionMenuDto>>
    {
        private readonly LocationSystem.Application.Contrats.Repositories.IUserRepository _userRepository;
        private readonly LocationSystem.Application.Contrats.Repositories.IMenuRepository _menuRepository;

        public GetUserPermissionMenusQueryHandler(LocationSystem.Application.Contrats.Repositories.IUserRepository userRepository, LocationSystem.Application.Contrats.Repositories.IMenuRepository menuRepository)
        {
            _userRepository = userRepository;
            _menuRepository = menuRepository;
        }

        public async Task<List<PermissionMenuDto>> Handle(GetUserPermissionMenusQuery request)
        {
            // 获取用户及其角色和权限
            var user = await _userRepository.GetByIdAsync(request.UserId);
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
            var allMenus = await _menuRepository.GetAll();
            // 这里简化处理，实际应该根据权限ID过滤菜单
            var userMenus = allMenus;

            // 转换为DTO
            var menuDtos = userMenus
                .Select(m => {
                    // 获取菜单关联的第一个权限代码作为菜单的权限标识
                    string? permissionCode = null;
                    
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
