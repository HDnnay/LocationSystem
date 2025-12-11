using LocationSystem.Domain.Enums;
using LocationSystem.Domain.Exceptions;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid DentistId { get; private set; }
        public Guid DentalOfficeId { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public Patient? Patient { get; private set; } 
        public DentalOffice? DentalOffice { get; private set; }
        public Dentist? Dentist { get; private set; }
        public TimeInterval TimeInterval { get; private set; }
        public Appointment(Guid patientId,Guid dentistId,Guid dentalOfficeId, TimeInterval timeInterval) 
        {
            if (timeInterval.Start < DateTime.UtcNow)
            {
                throw new BussinessRuleException("预约时间不能早于当前时间");
            }
            PatientId = patientId;
            DentistId = dentistId;
            DentalOfficeId  =  dentalOfficeId;
            Status = AppointmentStatus.Scheduled;
            TimeInterval = timeInterval;
            Id = Guid.NewGuid();
        }
        public void Cancel()
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                throw new BussinessRuleException("只有预约过才能被取消");
            }
            Status = AppointmentStatus.Canceled;

        }
        public void Complete()
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                throw new BussinessRuleException("只有预约过才能被完成");
            }
            Status = AppointmentStatus.Completed;
        }
    }
}
