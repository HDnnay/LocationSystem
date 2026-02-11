using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;
using LocationSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Menu>> GetMenusByPermissionIdsAsync(List<Guid> permissionIds)
        {
            return await _context.Menus
                .Where(m => m.PermissionMenus.Any(pm => permissionIds.Contains(pm.PermissionId)))
                .OrderBy(m => m.Order)
                .ToListAsync();
        }
    }
}
