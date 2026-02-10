using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail
{
    public class GetDentalOffcesDetailQuery:IRequest<DentalOfficesDetailDto>
    {
        public required Guid Id { get; set; }
    }
}
