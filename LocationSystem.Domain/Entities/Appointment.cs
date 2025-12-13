using LocationSystem.Domain.Enums;
using LocationSystem.Domain.Exceptions;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    /// <summary>
    /// 预约
    /// </summary>
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
        private Appointment() { }
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
        public void UpdateStatus(AppointmentStatus status)
        {
            switch (status)
            {
                case AppointmentStatus.Scheduled:
                    if (Status == status)
                        break;
                    this.SetScheduled();
                    break;
                case AppointmentStatus.Progress:
                    if (Status == status)
                        break;
                    this.SetProgress();
                    break;
                case AppointmentStatus.Completed:
                    if (Status == status)
                        break;
                    this.SetCompleted();
                    break;
                case AppointmentStatus.Canceled:
                    if (Status == status)
                        break;
                    SetCanceled();
                    break;
            }


        }
        public void UpdateTime(TimeInterval timeInterval)
        {
            TimeInterval = timeInterval;
        }
        /// <summary>
        /// 更改牙医医师
        /// </summary>
        /// <param name="id"></param>
        public void UpdateDentistId(Guid id)
        {
            DentistId = id;
        }
        /// <summary>
        /// 更改牙科
        /// </summary>
        /// <param name="id"></param>
        public void UpdateDentalOfficeId(Guid id)
        {
            DentalOfficeId = id;
        }
        private void SetScheduled()
        {
            Status = AppointmentStatus.Scheduled;
        }
        private void SetCanceled()
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                throw new BussinessRuleException("只有预约过才能被取消");
            }
            Status = AppointmentStatus.Canceled;

        }
        private void SetCompleted()
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                throw new BussinessRuleException("只有预约过才能被完成");
            }
            Status = AppointmentStatus.Completed;
        }
        private void SetProgress()
        {
            if (Status != AppointmentStatus.Scheduled)
                throw new BussinessRuleException("只有预约过才能进行");
            Status = AppointmentStatus.Progress;
        }
        
    }
}
