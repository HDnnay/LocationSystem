using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Appointments.Queries.GetAppointmentList;
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
            return await _context.Appointments.Where(t => t.Id == id && t.Status == AppointmentStatus.Scheduled).AnyAsync();
        }
        new public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return await _context.Appointments
                .Include(t => t.DentalOffice)
                .Include(t => t.Dentist)
                .Include(t => t.Patient).FirstOrDefaultAsync(t=>t.Id==id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentPage(AppointmentListFilter filter)
        {
            var query = _context.Appointments.Include(t => t.Patient)
                .Include(t => t.DentalOffice)
                .Include(t => t.Dentist)
                .AsQueryable().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(filter.keyWord))
            {
                query = query.Where(t =>
                    (t.Patient != null && t.Patient.Name != null && t.Patient.Name.Contains(filter.keyWord)) ||
                    (t.Dentist != null && t.Dentist.Name != null && t.Dentist.Name.Contains(filter.keyWord)) ||
                    (t.DentalOffice != null && t.DentalOffice.Name != null && t.DentalOffice.Name.Contains(filter.keyWord))
                );
            }
            var retult = await query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            return retult;
        }
    }
}
