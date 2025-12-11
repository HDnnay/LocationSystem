using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Repositories
{
    public class DentalOfficeRepository : Repository<DentalOffice>, IDentalOfficeRepositoty
    {
        public DentalOfficeRepository(AppDbContext context) 
            : base(context)
        {
        }
    }
}
