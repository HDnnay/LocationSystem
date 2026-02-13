using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Contrats.Services
{
    public interface ICacheManagerService
    {
        // 清除用户相关缓存
        Task ClearUserCacheAsync(Guid userId);
        
        // 清除角色相关缓存
        Task ClearRoleCacheAsync(Guid roleId);
        
        // 清除权限相关缓存
        Task ClearPermissionCacheAsync();
        
        // 清除所有缓存
        Task ClearAllCacheAsync();
    }
}