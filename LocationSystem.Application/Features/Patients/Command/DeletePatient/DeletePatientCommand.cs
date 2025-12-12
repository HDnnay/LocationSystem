using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Command.DeletePatient
{
    public class DeletePatientCommand:IRequset
    {
        public Guid Id { get; set; }
    }
}
