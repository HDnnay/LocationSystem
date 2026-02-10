using LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList
{
    public class GetDetalOfficesListQuery: DentalOfficeListFilter,IRequest<PageResult<DentalOfficesListDto>>
    {
    }
}
