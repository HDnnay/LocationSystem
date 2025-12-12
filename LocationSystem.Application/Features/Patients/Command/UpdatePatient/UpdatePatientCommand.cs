using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Command.UpdatePatient
{
    public class UpdatePatientCommand:IRequset
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string? Email { get; set; }
    }
}
