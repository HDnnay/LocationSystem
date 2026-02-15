using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class PermissionMenuRepository : Repository<PermissionMenu>, IPermissionMenuRepository
    {
        private readonly AppDbContext _context;

        public PermissionMenuRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PermissionMenu>> GetByMenuIdAsync(Guid menuId)
        {
            return await _context.PermissionMenus
                .Where(pm => pm.MenuId == menuId)
                .ToListAsync();
        }
    }
}