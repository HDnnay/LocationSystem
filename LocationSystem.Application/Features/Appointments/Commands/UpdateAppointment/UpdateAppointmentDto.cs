using LocationSystem.Domain.Entities;
using LocationSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentDto
    {
        public required Guid PatientId { get; set; }
        public required Guid DentistId { get; set; }
        public required Guid DentalOfficeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
