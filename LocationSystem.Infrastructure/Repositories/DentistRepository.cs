using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Dentists.Queries.GetDentistList;
using LocationSystem.Domain.Entities;
using LocationSystem.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Repositories
{
    public class DentistRepository : Repository<Dentist>, IDentistRepository
    {
        private readonly AppDbContext _context;
        public DentistRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dentist>> GetDentistPage(DentistListFilterDto fiter)
        {
            var querable = _context.Dentists.AsQueryable();
            if (string.IsNullOrWhiteSpace(fiter.keyWord))
            {
                querable.Where(t => t.Email.Value.Contains(fiter.keyWord) || t.Name.Contains(fiter.keyWord));
            }
            return await querable.OrderBy(t => t.Name)
                .Paginate(fiter.Page, fiter.PageSize)
                .ToListAsync();
        }
    }
}
