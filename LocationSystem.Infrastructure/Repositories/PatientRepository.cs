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

        public async Task<IEnumerable<Patient>> GetPatientPage(PatiensFilterDto fiter)
        {
            return await _context.Patients.OrderBy(t=>t.Name)
                .Paginate(fiter.page,fiter.pageSize)
                .ToListAsync();
        }
    }
}
