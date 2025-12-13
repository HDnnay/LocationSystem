using LocationSystem.Application.Dtos;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.Enums;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Queries.GetAppointmentDetail
{
    public class AppointmentDetailDto
    {
        public Guid Id { get;set; }
        public AppointmentStatus Status { get;set; }
        public PatientDto? Patient { get;set; }
        public DentalOfficeDto? DentalOffice { get;set; }
        public DentistDto? Dentist { get;set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
