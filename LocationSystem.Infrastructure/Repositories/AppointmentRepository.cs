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
        public async Task<bool> AppointmentIsScheduled(Guid id)
        {
            return await _context.Appointments.Where(t => t.Id == id && t.Status == AppointmentStatus.Scheduled).AnyAsync();
        }
        public async Task<bool> AppointmentIsCanceled(Guid id)
        {
           return await _context.Appointments.Where(t => t.Id == id && t.Status == AppointmentStatus.Canceled).AnyAsync();
        }

        public async Task<bool> AppointmentIsCompleted(Guid id)
        {
            return await _context.Appointments.Where(t => t.Id == id && t.Status == AppointmentStatus.Completed).AnyAsync();
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

        public async Task<bool> AppointmentIsProgress(Guid id)
        {
            return await _context.Appointments.Where(t => t.Id == id && t.Status == AppointmentStatus.Progress).AnyAsync();
        }
        new public async Task<Appointment> GetByIdAsync(Guid id)
        {
            return await _context.Appointments
                .Include(t => t.DentalOffice)
                .Include(t => t.Dentist)
                .Include(t => t.Patient).FirstOrDefaultAsync(t=>t.Id==id);
        }
    }
}
