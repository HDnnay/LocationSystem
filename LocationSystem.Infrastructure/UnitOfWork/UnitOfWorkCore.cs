using LocationSystem.Application.Contrats.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.UnitOfWork
{
    public class UnitOfWorkCore : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWorkCore(AppDbContext context) 
        {
            _context = context; 
        }
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public Task Rollback()
        {
            return Task.CompletedTask;
        }
    }
}
