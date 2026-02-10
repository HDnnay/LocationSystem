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
                .ToListAsync();
        }
    }
}