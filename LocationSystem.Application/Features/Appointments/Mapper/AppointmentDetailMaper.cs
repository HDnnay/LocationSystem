using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Appointments.Queries.GetAppointmentDetail;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Mapper
{
    public static class AppointmentDetailMaper
    {
        public static AppointmentDetailDto MapToDto(this Appointment appointment) => new AppointmentDetailDto
        {
            Id = appointment.Id,
            Patient = appointment.Patient==null ?null: new PatientDto(appointment.Patient),
            DentalOffice = appointment.DentalOffice ==null ?null: new DentalOfficeDto(appointment.DentalOffice),
            Dentist = appointment.Dentist==null ? null : new DentistDto(appointment.Dentist),
            StartTime = appointment.TimeInterval.Start,
            EndTime = appointment.TimeInterval.End,
            Status = appointment.Status,
        };
    }
}
