using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Appointments.Queries.GetAppointmentDetail;
using LocationSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Queries.GetAppointmentList
{
    public class AppointmentListDto
    {
        public Guid Id { get; set; }
        public AppointmentStatus Status { get; set; }
        public PatientDto? Patient { get; set; }
        public DentalOfficeDto? DentalOffice { get; set; }
        public DentistDto? Dentist { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
