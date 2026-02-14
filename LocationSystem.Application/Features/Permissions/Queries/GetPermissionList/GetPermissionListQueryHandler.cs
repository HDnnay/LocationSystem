using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Permissions.Queries.GetPermissionList;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionList
{
    public class GetPermissionListQueryHandler : IRequestHandler<GetPermissionListQuery, PageResult<PermissionDto>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ICacheService _cacheService;

        public GetPermissionListQueryHandler(IPermissionRepository permissionRepository, ICacheService cacheService)
        {
            _permissionRepository = permissionRepository;
            _cacheService = cacheService;
        }

        public async Task<PageResult<PermissionDto>> Handle(GetPermissionListQuery request)
        {
            // 生成缓存键
            var cacheKey = CacheKeys.PermissionWithPage(request);

            // 从缓存中获取权限列表或创建缓存
            var permissionDtos = await _cacheService.GetOrCreateAsync<PageResult<PermissionDto>>(cacheKey, async (options) => {
                // 获取所有权限及其角色
                var dics = await _permissionRepository.GetPermissionsPage(request);
                var permissionsDic = dics.FirstOrDefault();
                var total = permissionsDic.Key;
                // 转换为DTO
                var model = permissionsDic.Value.Select(permission => new PermissionDto
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
                return new PageResult<PermissionDto>() { CurrentPage=request.Page, Total= total,Data=model };
            },600); // 缓存30分钟（1800秒）

            return permissionDtos!;
        }
    }
}