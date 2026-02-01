using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.RentHousies.Queries.QueryRentHouseList;
using LocationSystem.Domain.Entities;
using LocationSystem.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Infrastructure.Repositories
{
    public class RentHouseRepository : Repository<RentHouse>, IRentHouseRepository
    {
        private AppDbContext _context;
        public RentHouseRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Dictionary<int, IEnumerable<RentHouse>>> GetRentHousePage(GetRentHouseListFilter filter)
        {
            var query = _context.RentHousies.AsQueryable().AsNoTracking();
            var count = await query.CountAsync();
            var result = await query.Paginate(filter.Page,filter.PageSize).ToListAsync();
            var dic = new Dictionary<int, IEnumerable<RentHouse>>();
            dic.Add(count, result);
            return dic;
        }
    }
}
