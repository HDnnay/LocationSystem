using LocationSystem.Application.Features.Patients.Queries.GetPatienList;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<IEnumerable<Patient>> GetPatientPage(PatiensFilterDto fiter);
    }
}
