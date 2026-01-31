using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Companys.Queries.ReadConpany;
using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LocationSystem.Infrastructure.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly AppDbContext _context;
        public CompanyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetCompanyPage(CompanyFilter filter)
        {
            var query = _context.Companies.AsQueryable().AsNoTracking();
            var result = await query.Skip((filter.Page-1)*filter.PageSize).Take(filter.PageSize).ToListAsync();
            return result;
        }
    }
}
