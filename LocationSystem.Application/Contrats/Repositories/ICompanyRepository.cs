using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface ICompanyRepository: IRepository<Company>
    {
        Task<IEnumerable<Company>> GetDentalOfficePage();

    }
}
