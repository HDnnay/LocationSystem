using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IAppointmentRepository:IRepository<Appointment>
    {
        Task<bool> AppointmentIsExists(Guid dentistId,DateTime start,DateTime end);
    }
}
