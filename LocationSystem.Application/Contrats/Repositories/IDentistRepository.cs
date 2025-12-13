using LocationSystem.Application.Features.Dentists.Queries.GetDentistList;
using LocationSystem.Application.Features.Patients.Queries.GetPatienList;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IDentistRepository: IRepository<Dentist>
    {
        Task<IEnumerable<Dentist>> GetDentistPage(DentistListFiterDto fiter);

    }
}
