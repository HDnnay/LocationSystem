using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommand:IRequset
    {
        public Guid Id { get; set; }
        public required Guid DentistId { get; set; }
        public required Guid DentalOfficeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
