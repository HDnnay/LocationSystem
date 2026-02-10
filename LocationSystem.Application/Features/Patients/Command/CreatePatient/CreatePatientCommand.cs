using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Command.CreatePatient
{
    public class CreatePatientCommand:IRequest<Patient>
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
