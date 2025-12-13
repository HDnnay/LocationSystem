using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Dentists.Commands.UpdateDentist
{
    public class UpdateDentistCommand:IRequset
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }

    }
}
