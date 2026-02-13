using LocationSystem.Application.Contrats.Services;
using LocationSystem.Application.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Events.Handlers
{
    public class CacheClearHandler
    {
        private readonly ICacheManagerService _cacheManagerService;
        
        public CacheClearHandler(ICacheManagerService cacheManagerService)
        {
            _cacheManagerService = cacheManagerService;
        }
        
        // 处理角色权限变更事件
        public async Task Handle(RolePermissionsChangedEvent @event)
        {
            await _cacheManagerService.ClearRoleCacheAsync(@event.RoleId);
        }
        
        // 处理用户角色变更事件
        public async Task Handle(UserRolesChangedEvent @event)
        {
            await _cacheManagerService.ClearUserCacheAsync(@event.UserId);
        }
        
        // 处理权限变更事件
        public async Task Handle(PermissionsChangedEvent @event)
        {
            await _cacheManagerService.ClearPermissionCacheAsync();
        }
    }
}