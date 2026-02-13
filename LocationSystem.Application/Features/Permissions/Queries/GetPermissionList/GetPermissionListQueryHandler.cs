using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionList;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionList
{
    public class GetPermissionListQueryHandler : IRequestHandler<GetPermissionListQuery, List<PermissionDto>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ICacheService _cacheService;

        public GetPermissionListQueryHandler(IPermissionRepository permissionRepository, ICacheService cacheService)
        {
            _permissionRepository = permissionRepository;
            _cacheService = cacheService;
        }

        public async Task<List<PermissionDto>> Handle(GetPermissionListQuery request)
        {
            // 生成缓存键
            var cacheKey = "permissions:list";

            // 从缓存中获取权限列表或创建缓存
            var permissionDtos = await _cacheService.GetOrCreateAsync<List<PermissionDto>>(cacheKey, async (options) => {
                // 获取所有权限及其角色
                var permissions = await _permissionRepository.GetPermissionsWithRolesAsync();

                // 转换为DTO
                return permissions.Select(permission => new PermissionDto
                {
                    Id = permission.Id,
                    Name = permission.Name,
                    Code = permission.Code,
                    Description = permission.Description,
                    ParentId = permission.ParentId,
                    CreatedAt = permission.CreatedAt,
                    UpdatedAt = permission.UpdatedAt,
                    Roles = permission.Roles.Select(role => new RoleDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Code = role.Code,
                        Description = role.Description,
                        CreatedAt = role.CreatedAt,
                        UpdatedAt = role.UpdatedAt
                    }).ToList(),
                    ChildPermissions = permission.ChildPermissions.Select(cp => new PermissionDto
                    {
                        Id = cp.Id,
                        Name = cp.Name,
                        Code = cp.Code,
                        Description = cp.Description,
                        ParentId = cp.ParentId,
                        CreatedAt = cp.CreatedAt,
                        UpdatedAt = cp.UpdatedAt,
                        ChildPermissions = new List<PermissionDto>()
                    }).ToList()
                }).ToList();
            }, 1800); // 缓存30分钟（1800秒）

            return permissionDtos;
        }
    }
}