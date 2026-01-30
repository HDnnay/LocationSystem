using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly AppDbContext _context;
        public CompanyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<Company>> GetDentalOfficePage()
        {
            throw new NotImplementedException();
        }
    }
}
