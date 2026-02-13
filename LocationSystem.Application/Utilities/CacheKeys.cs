using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Utilities
{
    public static class CacheKeys
    {
        // 用户权限缓存键
        public static string UserPermissions(Guid userId) => $"user:permissions:{userId}";
        
        // 用户菜单缓存键
        public static string UserMenus(Guid userId) => $"user:permission_menus:{userId}";
        
        // 角色权限缓存键
        public static string RolePermissions(Guid roleId) => $"role:{roleId}:permissions";
        
        // 权限树缓存键
        public static string PermissionTree => "permissions:tree";
        
        // 带选中状态的权限树缓存键
        public static string PermissionTreeWithCheck(Guid? roleId) => 
            roleId.HasValue ? $"permissions:tree:check:{roleId.Value}" : "permissions:tree:check:null";
        
        // 权限列表缓存键
        public static string PermissionList => "permissions:list";
        
        // 省份公司数量缓存键
        public static string ProvinceCompanyCount => "count_provice";
    }
}