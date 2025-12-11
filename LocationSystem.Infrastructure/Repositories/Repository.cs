using LocationSystem.Application.Contrats.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public Task<T> AddAsync(T entity)
        {
            _context.Add(entity);
            return Task.FromResult(entity);
        }

        public Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public Task<T?> GetByIdAsync(Guid id)
        {
            return _context.Set<T>().FindAsync(id).AsTask();
        }

        public Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }
    }
}
