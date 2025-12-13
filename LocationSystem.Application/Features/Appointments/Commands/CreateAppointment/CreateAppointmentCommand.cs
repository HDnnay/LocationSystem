using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommand:IRequset<Guid>
    {
        public required Guid PatientId { get; set; }
        public required Guid DentistId { get; set; }
        public required Guid DentalOfficeId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
    }
}
