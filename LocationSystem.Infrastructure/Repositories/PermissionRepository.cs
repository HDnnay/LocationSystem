using LocationSystem.Application.Contrats.Repositories;
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