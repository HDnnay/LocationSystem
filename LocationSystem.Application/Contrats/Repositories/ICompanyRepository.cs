using LocationSystem.Application.Features.Companys.Queries.ReadConpany;
using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface ICompanyRepository: IRepository<Company>
    {
        Task<Dictionary<int,IEnumerable<Company>>> GetCompanyPage(CompanyFilter filter);

    }
}
