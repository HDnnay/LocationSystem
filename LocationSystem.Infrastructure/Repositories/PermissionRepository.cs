using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities.Common;
using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private readonly AppDbContext _context;

        public PermissionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Permission?> GetByNameAsync(string name)
        {
            return await _context.Permissions.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<Permission?> GetByCodeAsync(string code)
        {
            return await _context.Permissions.FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<Permission?> GetPermissionWithRolesAsync(Guid id)
        {
            return await _context.Permissions
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<Permission>> GetPermissionsWithRolesAsync()
        {
            return await _context.Permissions
                .Include(p => p.Roles)
                .Include(p => p.ChildPermissions)
                .ToListAsync();
        }

        public async Task<Dictionary<int, IEnumerable<Permission>>> GetPermissionsPage(PageRequest pageRequest)
        {

            var query = _context.Permissions.AsNoTracking().AsQueryable();
            var count =await query.CountAsync();
            if (!string.IsNullOrWhiteSpace(pageRequest.KeyWord))
            {
                query = query.Where(t => t.Code.Contains(pageRequest.KeyWord)||t.Name.Contains(pageRequest.KeyWord));
            }
            var result =await query.Skip(pageRequest.PageSize*(pageRequest.Page-1)).Take(pageRequest.PageSize).ToListAsync();
            Dictionary<int, IEnumerable<Permission>> data = new Dictionary<int, IEnumerable<Permission>>();
            data.Add(count,result);
            return data;
        }

        public async Task<IEnumerable<Permission>> GetPermissionTreeAsync()
        {
            // 获取所有根权限（没有父权限的权限），并包含其所有子权限
            return await _context.Permissions
                .Where(p => p.ParentId == null)
                .Include(p => p.ChildPermissions)
                    .ThenInclude(cp => cp.ChildPermissions)
                .Include(p => p.ChildPermissions)
                    .ThenInclude(cp => cp.Roles)
                .Include(p => p.Roles)
                .ToListAsync();
        }

        public async Task<List<PermissionTreeDto>> GetPermissionTreeDtosAsync()
        {
            // 直接从数据库查询并构建专门的权限树DTO，只包含前端需要的字段
            // 使用Select投影优化查询，只加载需要的字段
            return await _context.Permissions
                .Where(p => p.ParentId == null)
                .Select(p => new PermissionTreeDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    DisplayName = p.Name,
                    Code = p.Code,
                    ParentId = p.ParentId,
                    IsCheck = false,
                    ChildPermissions = p.ChildPermissions.Select(cp => new PermissionTreeDto
                    {
                        Id = cp.Id,
                        Name = cp.Name,
                        DisplayName = cp.Name,
                        Code = cp.Code,
                        ParentId = cp.ParentId,
                        IsCheck = false,
                        ChildPermissions = cp.ChildPermissions.Select(ccp => new PermissionTreeDto
                        {
                            Id = ccp.Id,
                            Name = ccp.Name,
                            DisplayName = ccp.Name,
                            Code = ccp.Code,
                            ParentId = ccp.ParentId,
                            IsCheck = false
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<PermissionTreeDto>> GetPermissionTreeWithCheckStatusAsync(Guid? roleId)
        {
            // 先获取完整的权限树
            var permissionTree = await GetPermissionTreeDtosAsync();
            
            if (!roleId.HasValue)
            {
                // 如果没有提供角色ID，返回所有权限未选中的状态
                return permissionTree;
            }
            
            // 获取角色拥有的权限ID列表
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId.Value);
            
            var rolePermissionIds = role?.Permissions.Select(p => p.Id).ToList() ?? new List<Guid>();
            
            // 递归设置权限的选中状态
            SetCheckStatus(permissionTree, rolePermissionIds);
            
            return permissionTree;
        }

        private void SetCheckStatus(List<PermissionTreeDto> permissions, List<Guid> rolePermissionIds)
        {
            foreach (var permission in permissions)
            {
                // 设置当前权限的选中状态
                permission.IsCheck = rolePermissionIds.Contains(permission.Id);
                
                // 递归处理子权限
                if (permission.ChildPermissions.Any())
                {
                    SetCheckStatus(permission.ChildPermissions, rolePermissionIds);
                }
            }
        }

        public async Task<Permission?> GetPermissionWithChildrenAsync(Guid id)
        {
            // 获取指定权限及其所有子权限
            return await _context.Permissions
                .Include(p => p.ChildPermissions)
                    .ThenInclude(cp => cp.ChildPermissions)
                .Include(p => p.ChildPermissions)
                    .ThenInclude(cp => cp.Roles)
                .Include(p => p.Roles)
                .Include(p => p.Parent)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}