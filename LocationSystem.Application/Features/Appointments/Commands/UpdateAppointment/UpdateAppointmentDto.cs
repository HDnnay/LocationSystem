using LocationSystem.Domain.Entities;
using LocationSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentDto
    {
        /// <summary>
        /// 理论上预约单跟患者是关联的，患者ID被更改这个预约单是不存在了，所以患者不能更改
        /// </summary>
        public required Guid DentistId { get; set; }
        public required Guid DentalOfficeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
