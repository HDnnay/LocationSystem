using LocationSystem.Domain.Entities.DeletedSnapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IDeletedSnapshotRepository
    {
        Task<DeletedSnapshot> AddAsync(DeletedSnapshot snapshot);
        Task<DeletedSnapshot?> GetByIdAsync(int id);
        IQueryable<DeletedSnapshot> Query();
        Task<List<DeletedSnapshot>> FindAsync(Expression<Func<DeletedSnapshot, bool>> predicate);
        Task<List<DeletedSnapshot>> GetByEntityTypeAndIdAsync(string entityType, string entityId);
    }
}
