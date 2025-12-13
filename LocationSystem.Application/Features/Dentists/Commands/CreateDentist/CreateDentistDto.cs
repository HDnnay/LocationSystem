using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Dentists.Commands.CreateDentist
{
    public class CreateDentistDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
