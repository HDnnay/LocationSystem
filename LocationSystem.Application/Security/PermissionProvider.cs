// LocationSystem.Application/Security/PermissionProvider.cs
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Core.Security.Abstractions;
using Microsoft.Extensions.Logging;

namespace LocationSystem.Application.Security
{
    public class PermissionProvider : IPermissionProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionMenuRepository _permissionMenuRepository;
        private readonly ILogger<PermissionProvider> _logger;

        public PermissionProvider(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IPermissionMenuRepository permissionMenuRepository,
            ILogger<PermissionProvider> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _permissionMenuRepository = permissionMenuRepository;
            _logger = logger;
        }

        public async Task<List<string>> GetUserPermissionCodesAsync(Guid userId)
        {
            try
            {
                // 1. 获取用户角色权限
                var rolePermissions = await GetUserRolePermissionCodesAsync(userId);

                // 2. 获取用户直接权限
                var directPermissions = await GetUserDirectPermissionCodesAsync(userId);

                // 3. 合并权限并去重
                var allPermissions = rolePermissions
                    .Union(directPermissions)
                    .Distinct()
                    .ToList();

                _logger.LogDebug("用户 {UserId} 拥有 {Count} 个权限", userId, allPermissions.Count);

                return allPermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户权限代码失败: {UserId}", userId);
                throw;
            }
        }

        public async Task<List<string>> GetUserRolePermissionCodesAsync(Guid userId)
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

        public async Task<List<string>> GetUserDirectPermissionCodesAsync(Guid userId)
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
                var superAdminRoleCode = GetSuperAdminRoleCode();

                return userRoles?.Any(role => role?.Code == superAdminRoleCode) == true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查超级管理员权限失败: {UserId}", userId);
                return false;
            }
        }

        public string GetSuperAdminRoleCode()
        {
            return "admin"; // 可以从配置中读取
        }

        public async Task<Dictionary<string, List<Guid>>> GetUserDataLevelPermissionsAsync(Guid userId)
        {
            // 实现数据级权限查询
            // 返回格式: { "Article": [guid1, guid2], "User": [guid3] }
            return new Dictionary<string, List<Guid>>();
        }

        public async Task<bool> HasDataLevelPermissionAsync(Guid userId, string resourceType, Guid resourceId)
        {
            var dataLevelPermissions = await GetUserDataLevelPermissionsAsync(userId);

            if (dataLevelPermissions.TryGetValue(resourceType, out var allowedResourceIds))
            {
                return allowedResourceIds.Contains(resourceId);
            }

            return false;
        }

        public async Task RefreshUserPermissionsCacheAsync(Guid userId)
        {
            // 实现权限缓存刷新逻辑
            _logger.LogInformation("刷新用户权限缓存: {UserId}", userId);
        }

        public Task<List<string>> GetUserPermissionsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}