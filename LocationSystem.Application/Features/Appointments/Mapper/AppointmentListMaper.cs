using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Appointments.Queries.GetAppointmentList;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Mapper
{
    public static class AppointmentListMaper
    {
        public static AppointmentListDto MapToListDto(this Appointment appointment)
        {
            return new AppointmentListDto()
            {
                Id = appointment.Id,
                Patient = appointment.Patient == null ? null : new PatientDto(appointment.Patient),
                DentalOffice = appointment.DentalOffice == null ? null : new DentalOfficeDto(appointment.DentalOffice),
                Dentist = appointment.Dentist == null ? null : new DentistDto(appointment.Dentist),
                StartTime = appointment.TimeInterval.Start,
                EndTime = appointment.TimeInterval.End,
                Status = appointment.Status,
            };
        }
    }
}
