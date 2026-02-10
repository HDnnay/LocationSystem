using LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Companys.Queries.ReadConpany
{
    public class ReadConpanyQuery: CompanyFilter, IRequest<PageResult<CompanyDto>>
    {
    }
}
