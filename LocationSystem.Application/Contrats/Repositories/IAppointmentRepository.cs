using LocationSystem.Application.Features.Appointments.Queries.GetAppointmentList;
using LocationSystem.Application.Features.Patients.Queries.GetPatienList;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IAppointmentRepository:IRepository<Appointment>
    {
        Task<bool> AppointmentIsExists(Guid dentistId,DateTime start,DateTime end);
        Task<bool> AppointmentIsCompleted(Guid id);
        Task<bool> AppointmentIsCanceled(Guid id);
        Task<bool> AppointmentIsProgress(Guid id);
        Task<bool> AppointmentIsScheduled(Guid id);
        new Task<Appointment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Appointment>> GetAppointmentPage(AppointmentListFilter fiter);
    }
}
