using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail
{
    public class DentalOfficesDetailDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; } = string.Empty;
    }
}
