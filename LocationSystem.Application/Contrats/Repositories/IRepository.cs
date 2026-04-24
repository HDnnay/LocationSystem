using System.Linq.Expressions;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        ParallelQuery<T> QueryAsParalle();
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        void UpdateRang(IEnumerable<T> entities);
        Task AddRangAsync(List<T> entities);
        Task DeleteAsync(T entity);
        Task<int> GetTotalCount(Expression<Func<T, bool>> predicate = null);
        Task<IEnumerable<TResult>> GetAllFromSelectedFields<TResult>(Expression<Func<T, TResult>> selector);

    }
}
