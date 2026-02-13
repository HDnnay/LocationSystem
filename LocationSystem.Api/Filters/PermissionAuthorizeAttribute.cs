using LocationSystem.Application.Utilities;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;using Microsoft.AspNetCore.Mvc.Filters;using Microsoft.Extensions.Caching.Distributed;using System.Security.Claims;using System.Threading.Tasks;

namespace LocationSystem.Api.Filters
{
    /// <summary>
    /// 权限认证过滤器
    /// </summary>
    public class PermissionAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// 权限代码
        /// </summary>
        public string PermissionCode { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="permissionCode">权限代码</param>
        public PermissionAuthorizeAttribute(string permissionCode)
        {
            PermissionCode = permissionCode;
        }

        /// <summary>
        /// 异步授权过滤器
        /// </summary>
        /// <param name="context">授权过滤器上下文</param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // 获取用户ID
            var userId = context.HttpContext.User.GetUserId();
            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // 获取缓存服务
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            
            // 生成缓存键
            var cacheKey = CacheKeys.UserPermissions(userId.Value);
            
            // 从缓存中获取用户权限或创建缓存
            var userPermissions = await cacheService.GetOrCreateAsync<List<string>>(cacheKey, async (options) => {
                // 获取权限管理服务
                var permissionManagement = context.HttpContext.RequestServices.GetRequiredService<LocationSystem.Application.Services.PermissionManagement>();
                
                // 获取用户的所有权限代码
                return await permissionManagement.GetUserPermissionCodesAsync(userId.Value);
            }, 1800); // 缓存30分钟（1800秒）
            
            // 检查用户是否是超级管理员（拥有admin角色）
            var roleRepository = context.HttpContext.RequestServices.GetRequiredService<LocationSystem.Application.Contrats.Repositories.IRoleRepository>();
            var userRoles = await roleRepository.GetRolesByUserIdAsync(userId.Value);
            var isSuperAdmin = userRoles.Any(role => role != null && role.Code == "admin");
            
            // 如果是超级管理员，直接通过
            if (isSuperAdmin)
            {
                return;
            }
            
            // 检查用户是否有权限
            if (userPermissions == null || !userPermissions.Contains(PermissionCode))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
