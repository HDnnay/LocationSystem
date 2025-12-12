using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Commands.UpDentalOffice
{
    public class UpdateDetalOfficeCommand:IRequset
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
