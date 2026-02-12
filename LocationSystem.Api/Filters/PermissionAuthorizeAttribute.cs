using LocationSystem.Application.Utilities;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;using Microsoft.AspNetCore.Mvc.Filters;using Microsoft.Extensions.Caching.Distributed;using Newtonsoft.Json;using System.Security.Claims;using System.Threading.Tasks;

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

            // 获取分布式缓存
            var distributedCache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
            
            // 生成缓存键
            var cacheKey = $"user:permissions:{userId.Value}";
            
            // 从缓存中获取用户权限
            List<string> userPermissions = null;
            var cachedPermissions = await distributedCache.GetStringAsync(cacheKey);
            
            if (!string.IsNullOrEmpty(cachedPermissions))
            {
                try
                {
                    userPermissions = JsonConvert.DeserializeObject<List<string>>(cachedPermissions);
                }
                catch (Exception ex)
                {
                    // 缓存反序列化失败，忽略错误，继续从数据库获取
                    Console.WriteLine($"缓存反序列化失败: {ex.Message}");
                }
            }
            
            // 如果缓存中没有权限，从数据库获取
            if (userPermissions == null)
            {
                // 获取权限管理服务
                var permissionManagement = context.HttpContext.RequestServices.GetRequiredService<LocationSystem.Application.Services.PermissionManagement>();
                
                // 获取用户的所有权限代码
                userPermissions = await permissionManagement.GetUserPermissionCodesAsync(userId.Value);
                
                // 将权限存储到缓存
                if (userPermissions != null && userPermissions.Any())
                {
                    var permissionsJson = JsonConvert.SerializeObject(userPermissions);
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // 缓存30分钟
                    };
                    await distributedCache.SetStringAsync(cacheKey, permissionsJson, cacheOptions);
                }
            }
            
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
