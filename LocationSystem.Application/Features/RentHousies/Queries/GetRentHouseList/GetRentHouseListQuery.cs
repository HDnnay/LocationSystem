using LocationSystem.Application.Features.RentHousies.Queries.GetRentHouseList;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.RentHousies.Queries.QueryRentHouseList
{
    public class GetRentHouseListQuery:GetRentHouseListFilter,IRequest<PageResult<RentHouseListDto>>
    {
    }
}
