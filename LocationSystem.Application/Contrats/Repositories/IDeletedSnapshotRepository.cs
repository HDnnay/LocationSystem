using LocationSystem.Application.Dtos.Snapshots;
using LocationSystem.Domain.Entities.DeletedSnapshots;
using System.Linq.Expressions;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IDeletedSnapshotRepository
    {
        Task<DeletedSnapshot> AddAsync(DeletedSnapshot snapshot);
        Task<DeletedSnapshot?> GetByIdAsync(int id);
        IQueryable<DeletedSnapshot> Query();
        Task<List<DeletedSnapshot>> FindAsync(Expression<Func<DeletedSnapshot, bool>> predicate);
        Task<List<DeletedSnapshot>> GetByEntityTypeAndIdAsync(string entityType, string entityId);
        Task<(int, IEnumerable<DeletedSnapshotDto>)> GetPageAsync(int page = 1, int pageSize = 10, Expression<Func<DeletedSnapshot, bool>>? predicate = null);
    }
}
