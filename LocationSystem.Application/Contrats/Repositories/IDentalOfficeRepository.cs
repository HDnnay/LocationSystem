using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Application.Features.Patients.Queries.GetPatienList;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IDentalOfficeRepository:IRepository<DentalOffice>
    {
        Task<IEnumerable<DentalOffice>> GetDentalOfficePage(DentalOfficeListFilter fiter);
    }
}
