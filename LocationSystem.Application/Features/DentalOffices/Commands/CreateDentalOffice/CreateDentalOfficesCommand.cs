using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Commands.CreateDentalOffice
{
    public class CreateDentalOfficesCommand:IRequest<Guid>
    {
        public required string Name { get; set; }
    }
}
