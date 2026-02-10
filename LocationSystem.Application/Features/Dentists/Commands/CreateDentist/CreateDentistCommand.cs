using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Dentists.Commands.CreateDentist
{
    public class CreateDentistCommand : IRequest<Guid>
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
