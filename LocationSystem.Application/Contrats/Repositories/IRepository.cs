using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task UpdateRangAsync(List<T> entities);
        Task DeleteAsync(T entity);
        Task<int> GetTotalCount(Expression<Func<T, bool>> predicate=null);

    }
}
