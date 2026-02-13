using LocationSystem.Application.Contrats.Services;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Services
{
    public class CacheManagerService : ICacheManagerService
    {
        private readonly ICacheService _cacheService;
        
        public CacheManagerService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        
        // 清除用户相关缓存
        public Task ClearUserCacheAsync(Guid userId)
        {
            // 清除用户权限缓存
            _cacheService.Remove(CacheKeys.UserPermissions(userId));
            
            // 清除用户菜单缓存
            _cacheService.Remove(CacheKeys.UserMenus(userId));
            
            return Task.CompletedTask;
        }
        
        // 清除角色相关缓存
        public async Task ClearRoleCacheAsync(Guid roleId)
        {
            // 清除角色权限缓存
            _cacheService.Remove(CacheKeys.RolePermissions(roleId));
            
            // 清除带选中状态的权限树缓存
            _cacheService.Remove(CacheKeys.PermissionTreeWithCheck(roleId));
            
            // 清除所有用户的权限缓存
            await _cacheService.RemoveByPatternAsync("user:*:permissions");
            await _cacheService.RemoveByPatternAsync("user:*:menus");
        }
        
        // 清除权限相关缓存
        public async Task ClearPermissionCacheAsync()
        {
            // 清除权限树缓存
            _cacheService.Remove(CacheKeys.PermissionTree);
            
            // 清除所有带选中状态的权限树缓存
            await _cacheService.RemoveByPatternAsync("permissions:tree:check:*");
            
            // 清除所有角色权限缓存
            await _cacheService.RemoveByPatternAsync("role:*:permissions");
            
            // 清除所有用户的权限缓存
            await _cacheService.RemoveByPatternAsync("user:*:permissions");
            await _cacheService.RemoveByPatternAsync("user:*:menus");
        }
        
        // 清除所有缓存
        public async Task ClearAllCacheAsync()
        {
            await _cacheService.ClearAllAsync();
        }
    }
}