using LocationSystem.Application.Services;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetUserPermissions
{
    public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, List<string>>
    {
        private readonly PermissionManagement _permissionManagement;
        private readonly ICacheService _cacheService;

        public GetUserPermissionsQueryHandler(PermissionManagement permissionManagement, ICacheService cacheService)
        {
            _permissionManagement = permissionManagement;
            _cacheService = cacheService;
        }

        public async Task<List<string>> Handle(GetUserPermissionsQuery request)
        {
            // 生成缓存键
            var cacheKey = $"user:permissions:{request.UserId}";

            // 从缓存中获取用户权限代码或创建缓存
            var permissionCodes = await _cacheService.GetOrCreateAsync<List<string>>(cacheKey, async (options) => {
                // 获取用户的所有权限代码
                return await _permissionManagement.GetUserPermissionCodesAsync(request.UserId);
            }, 1800); // 缓存30分钟（1800秒）

            return permissionCodes;
        }
    }
}