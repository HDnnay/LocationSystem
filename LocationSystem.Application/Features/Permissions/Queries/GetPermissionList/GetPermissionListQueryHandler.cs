using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Permissions;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using Mapster;

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
            var permissionDtos = await _cacheService.GetOrCreateAsync(cacheKey, async (options) =>
            {
                // 获取所有权限及其角色
                var dics = await _permissionRepository.GetPermissionsPage(request);
                var permissionsDic = dics.FirstOrDefault();
                var total = permissionsDic.Key;
                // 转换为DTO
                var model = permissionsDic.Value.Select(permission => permission.Adapt<PermissionDto>()).ToList();
                return new PageResult<PermissionDto>() { CurrentPage=request.Page, Total= total, Items=model };
            }, 600); // 缓存30分钟（1800秒）

            return permissionDtos!;
        }
    }
}