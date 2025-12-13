using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AppointmentIsExists(Guid dentistId, DateTime start, DateTime end)
        {
            var reult =await _context.Appointments
                .Where(t=>t.DentistId==dentistId
                &&t.Status ==AppointmentStatus.Scheduled
                &&start<t.TimeInterval.End
                &&end>t.TimeInterval.Start).AnyAsync();
            return reult;
        }
    }
}
