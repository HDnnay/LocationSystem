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
            // 先获取与权限关联的所有菜单
            var userMenus = await _context.Menus
                .Where(m => m.PermissionMenus.Any(pm => permissionIds.Contains(pm.PermissionId)))
                .Include(m => m.PermissionMenus)
                .ThenInclude(pm => pm.Permission)
                .OrderBy(m => m.Order)
                .ToListAsync();

            // 收集所有菜单的父菜单ID
            var parentIds = userMenus
                .Where(m => m.ParentId.HasValue)
                .Select(m => m.ParentId.Value)
                .Distinct()
                .ToList();

            // 如果有父菜单ID，获取这些父菜单
            if (parentIds.Any())
            {
                var parentMenus = await _context.Menus
                    .Where(m => parentIds.Contains(m.Id))
                    .Include(m => m.PermissionMenus)
                    .ThenInclude(pm => pm.Permission)
                    .OrderBy(m => m.Order)
                    .ToListAsync();

                // 将父菜单添加到用户菜单列表中
                userMenus.AddRange(parentMenus);
            }

            return userMenus.DistinctBy(m => m.Id).OrderBy(m => m.Order).ToList();
        }

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            return await _context.Menus
                .Include(m => m.Children)
                .Include(m => m.PermissionMenus)
                .ThenInclude(pm => pm.Permission)
                .OrderBy(m => m.Order)
                .ToListAsync();
        }

        public async Task<Menu?> GetMenuByIdAsync(Guid id)
        {
            return await _context.Menus
                .Include(m => m.Children)
                .Include(m => m.PermissionMenus)
                .ThenInclude(pm => pm.Permission)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Menu> CreateMenuAsync(Menu menu)
        {
            await _context.Menus.AddAsync(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task UpdateMenuAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMenuAsync(Guid id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
        }
    }
}
