using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Permissions.Models;
using LocationSystem.Application.Services;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetUserPermissionMenus
{
    public class GetUserPermissionMenusQueryHandler : IRequestHandler<GetUserPermissionMenusQuery, List<PermissionMenuDto>>
    {
        private readonly PermissionManagement _permissionManagement;
        private readonly ICacheService _cacheService;

        public GetUserPermissionMenusQueryHandler(PermissionManagement permissionManagement, ICacheService cacheService)
        {
            _permissionManagement = permissionManagement;
            _cacheService = cacheService;
        }

        public async Task<List<PermissionMenuDto>> Handle(GetUserPermissionMenusQuery request)
        {
            // 生成缓存键
            var cacheKey = CacheKeys.UserMenus(request.UserId);

            // 从缓存中获取用户权限菜单或创建缓存
            var menuDtos = await _cacheService.GetOrCreateAsync<List<PermissionMenuDto>>(cacheKey, async (options) => {
                // 使用PermissionManagement获取用户有权限的菜单
                var userMenus = await _permissionManagement.GetUserPermissionMenusAsync(request.UserId);
                if (!userMenus.Any())
                {
                    return new List<PermissionMenuDto>();
                }

                // 转换为DTO
                var dtos = userMenus
                    .Select(m => {
                        // 获取菜单关联的第一个权限代码作为菜单的权限标识
                        string? permissionCode = null;
                        var firstPermissionMenu = m.PermissionMenus.FirstOrDefault();
                        if (firstPermissionMenu != null)
                        {
                            // 这里简化处理，实际应该通过权限ID获取权限信息
                            permissionCode = "";
                        }
                        
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
                return BuildMenuTree(dtos);
            }, 1800); // 缓存30分钟（1800秒）

            return menuDtos!;
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
