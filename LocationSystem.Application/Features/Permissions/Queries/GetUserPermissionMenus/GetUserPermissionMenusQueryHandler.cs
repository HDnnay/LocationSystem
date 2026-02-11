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

            // 获取与这些权限关联的所有菜单
            var userMenus = await _menuRepository.GetMenusByPermissionIdsAsync(userPermissionIds);

            // 转换为DTO
            var menuDtos = userMenus
                .Select(m => new PermissionMenuDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Code = null,
                    Description = null,
                    ParentId = m.ParentId,
                    IsMenu = true,
                    MenuPath = m.Path,
                    MenuIcon = m.Icon,
                    Order = m.Order,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt ?? m.CreatedAt
                })
                .ToList();

            return menuDtos;
        }
    }
}
