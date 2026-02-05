using LocationSystem.Application.Contrats.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<int> GetTotalCount(Expression<Func<T, bool>> predicate=null)
        {
            if (predicate != null)
            {
                return await _context.Set<T>().CountAsync(predicate);
            }
            return await _context.Set<T>().CountAsync();
        }

        public Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }

        public async Task AddRangAsync(List<T> entities)
        {
            if (entities.Any())
              await  _context.AddRangeAsync(entities);
        }

        public void UpdateRang(IEnumerable<T> entities)
        {
            if (entities.Any())
            {
                _context.UpdateRange(entities);
                
            }
        }
    }
}
