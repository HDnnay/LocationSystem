using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Patients.Queries.GetPatienList;
using LocationSystem.Domain.Entities;
using LocationSystem.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly AppDbContext _context;
        public PatientRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetPatientPage(PatiensListFilterDto fiter)
        {
            var querable = _context.Patients.AsQueryable();
            if (string.IsNullOrWhiteSpace(fiter.keyWord))
            {
                querable.Where(t=>t.Email.Value.Contains(fiter.keyWord)||t.Name.Contains(fiter.keyWord));
            }
            return await querable.OrderBy(t=>t.Name)
                .Paginate(fiter.Page,fiter.PageSize)
                .ToListAsync();
        }
    }
}
