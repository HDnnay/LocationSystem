using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentCommand:IRequset
    {
        public Guid Id { get; set; }
    }
}
