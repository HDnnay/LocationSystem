using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Menus.Queries.GetAllMenus;
using LocationSystem.Domain.Entities;
using LocationSystem.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly AppDbContext _context;

        public MenuRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Menu>> GetMenuPage(GetAllMenusQuery query)
        {
            var querable = _context.Menus.AsQueryable().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(query.keyWord))
            {
                querable = querable.Where(t => t.Name.Contains(query.keyWord) || t.Path.Contains(query.keyWord));
            }
            return await querable.OrderBy(t => t.Order)
                .Paginate(query.Page, query.PageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCount()
        {
            return await _context.Menus.CountAsync();
        }

        public async Task<IEnumerable<Menu>> GetMenuTreeAsync()
        {
            // 获取所有根菜单（没有父菜单的菜单），并包含其所有子菜单
            return await _context.Menus
                .Where(m => m.ParentId == null)
                .Include(m => m.Children)
                    .ThenInclude(cm => cm.Children)
                .OrderBy(m => m.Order)
                .ToListAsync();
        }
    }
}
