using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Repositories
{
    public class DentalOfficeRepository : Repository<DentalOffice>, IDentalOfficeRepository
    {
        private readonly AppDbContext _context;
        public DentalOfficeRepository(AppDbContext context) 
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DentalOffice>> GetDentalOfficePage(DentalOfficeListFilter fiter)
        {
            var query = _context.DentalOffices.AsQueryable();
            var result = await query.Skip((fiter.Page-1)*fiter.PageSize).Take(fiter.PageSize).ToListAsync();
            return result;
        }
    }
}
