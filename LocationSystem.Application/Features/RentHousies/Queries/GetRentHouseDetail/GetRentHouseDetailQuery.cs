using LocationSystem.Application.Features.RentHousies.Queries.ShareDtos;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.RentHousies.Queries.GetRentHouseDetail
{
    public class GetRentHouseDetailQuery:IRequest<RentHouseDto>
    {
        public required Guid Id { get; set; }
    }
}
