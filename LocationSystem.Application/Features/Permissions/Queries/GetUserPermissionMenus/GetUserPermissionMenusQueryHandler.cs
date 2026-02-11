using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Application.Features.Permissions.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetUserPermissionMenus
{
    public class GetUserPermissionMenusQueryHandler : IRequestHandler<GetUserPermissionMenusQuery, List<PermissionMenuDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserPermissionMenusQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<PermissionMenuDto>> Handle(GetUserPermissionMenusQuery request)
        {
            // 获取用户及其角色和权限
            var user = await _userRepository.GetUserWithRolesAndPermissionsAsync(request.UserId);
            if (user == null)
            {
                return new List<PermissionMenuDto>();
            }

            // 获取用户角色的所有权限
            var userPermissions = new List<Permission>();
            foreach (var role in user.Roles)
            {
                foreach (var permission in role.Permissions)
                {
                    if (!userPermissions.Any(p => p.Id == permission.Id))
                    {
                        userPermissions.Add(permission);
                    }
                }
            }

            // 转换为DTO
            var menuDtos = userPermissions
                .Select(p => new PermissionMenuDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Description = p.Description,
                    ParentId = p.ParentId,
                    IsMenu = false,
                    MenuPath = string.Empty,
                    MenuIcon = string.Empty,
                    Order = 0,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt ?? p.CreatedAt
                })
                .ToList();

            return menuDtos;
        }
    }
}
