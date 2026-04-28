// LocationSystem.Application/Security/PermissionProvider.cs
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using LocationSystem.Core.Security.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LocationSystem.Application.Security
{
    public class PermissionProvider : IPermissionProvider
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<PermissionProvider> _logger;
        private readonly IServiceProvider _serviceProvider;
        public PermissionProvider(
            IRoleRepository roleRepository,
            IServiceProvider serviceProvider,
            ILogger<PermissionProvider> logger)
        {
            _roleRepository = roleRepository;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        private async Task<List<string>> GetUserPermissionCodesAsync(Guid userId)
        {
            try
            {
                var cacheService = _serviceProvider.GetRequiredService<ICacheService>();
                var cacheKey = CacheKeys.UserPermissions(userId);

                // 从缓存中获取用户权限或创建缓存
                var userPermissions = await cacheService.GetOrCreateAsync<List<string>>(cacheKey, async (options) =>
                {
                    // 获取权限管理服务
                    var permissionManagement = _serviceProvider.GetRequiredService<LocationSystem.Application.Services.PermissionManagement>();
                    // 获取用户的所有权限代码
                    return await permissionManagement.GetUserPermissionCodesAsync(userId);
                }, 1800);

                return userPermissions!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户权限代码失败: {UserId}", userId);
                throw;
            }
        }

        private async Task<List<string>> GetUserRolePermissionCodesAsync(Guid userId)
        {
            // 1. 获取用户角色
            var userRoles = await _roleRepository.GetRolesByUserIdAsync(userId);
            if (userRoles == null || !userRoles.Any())
            {
                return new List<string>();
            }

            // 2. 获取角色权限
            var rolePermissionCodes = new List<string>();
            foreach (var role in userRoles)
            {
                if (role?.Permissions != null)
                {
                    var rolePermissions = role.Permissions
                        .Where(p => p != null && !string.IsNullOrEmpty(p.Code))
                        .Select(p => p.Code)
                        .ToList();

                    rolePermissionCodes.AddRange(rolePermissions);
                }
            }

            return rolePermissionCodes.Distinct().ToList();
        }

        private async Task<List<string>> GetUserDirectPermissionCodesAsync(Guid userId)
        {
            // 实现获取用户直接分配的权限
            // 这里可以根据你的业务逻辑实现
            return new List<string>();
        }

        public async Task<bool> IsSuperAdminAsync(Guid userId)
        {
            try
            {
                var userRoles = await _roleRepository.GetRolesByUserIdAsync(userId);
                var isSuperAdmin = userRoles.Any(role => role != null && role.IsSuperAdmin);
                return isSuperAdmin;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查超级管理员权限失败: {UserId}", userId);
                return false;
            }
        }

        private string GetSuperAdminRoleCode()
        {
            return "admin"; // 可以从配置中读取
        }

        private async Task<Dictionary<string, List<Guid>>> GetUserDataLevelPermissionsAsync(Guid userId)
        {
            // 实现数据级权限查询
            // 返回格式: { "Article": [guid1, guid2], "User": [guid3] }
            return new Dictionary<string, List<Guid>>();
        }

        private async Task<bool> HasDataLevelPermissionAsync(Guid userId, string resourceType, Guid resourceId)
        {
            var dataLevelPermissions = await GetUserDataLevelPermissionsAsync(userId);

            if (dataLevelPermissions.TryGetValue(resourceType, out var allowedResourceIds))
            {
                return allowedResourceIds.Contains(resourceId);
            }

            return false;
        }

        private async Task RefreshUserPermissionsCacheAsync(Guid userId)
        {
            // 实现权限缓存刷新逻辑
            _logger.LogInformation("刷新用户权限缓存: {UserId}", userId);
        }

        public async Task<List<string>> GetUserPermissionsAsync(Guid userId)
        {
            return await GetUserPermissionCodesAsync(userId);
        }
    }
}