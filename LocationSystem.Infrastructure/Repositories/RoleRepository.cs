using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Domain.Entities.UserRolePermissions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<Role?> GetByCodeAsync(string code)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Code == code);
        }

        public async Task<Role?> GetRoleWithPermissionsAsync(Guid id)
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Role>> GetRolesWithPermissionsAsync()
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .ToListAsync();
        }

        public async Task<List<Role>> GetRolesWithPermissionsByIdsAsync(List<Guid> roleIds)
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId)
        {
            // 获取用户的所有角色，包含权限信息
            return await _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Roles)
                .Include(r => r.Permissions)
                .ToListAsync();
        }

        public Task<Dictionary<Guid, RoleGraphqLDto>> GetRoleByIds(IReadOnlyList<Guid> ids, CancellationToken cts = default)
        {
            return _context.Roles.Where(t => ids.Contains(t.Id)).Select(t => t.Adapt<RoleGraphqLDto>()).ToDictionaryAsync(t => t.Id);
        }

        public async Task<Dictionary<Guid, List<RoleGraphqLDto>>> GetRolesByUserIdsAsync(IReadOnlyList<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
                return new Dictionary<Guid, List<RoleGraphqLDto>>();

            // 查询用户角色关系，包含权限信息
            var userRoles = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new
                {
                    UserId = u.Id,
                    Roles = u.Roles.Select(r => r.Adapt<RoleGraphqLDto>()).ToList()
                })
                .ToListAsync();

            // 转换为字典格式
            return userRoles.ToDictionary(
                ur => ur.UserId,
                ur => ur.Roles
            );
        }
    }
}