using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList
{
    public class DentalOfficesListDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
