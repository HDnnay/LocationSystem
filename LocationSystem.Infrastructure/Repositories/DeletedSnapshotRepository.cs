using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.DeletedSnapshots;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LocationSystem.Infrastructure.Repositories
{
    public class DeletedSnapshotRepository : Repository<DeletedSnapshot>, IDeletedSnapshotRepository
    {
        private readonly AppDbContext _context;

        public DeletedSnapshotRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<DeletedSnapshot> Query()
        {
            return _context.DeletedSnapshots;
        }

        public async Task<List<DeletedSnapshot>> FindAsync(Expression<Func<DeletedSnapshot, bool>> predicate)
        {
            return await _context.DeletedSnapshots
                .Where(predicate)
                .OrderByDescending(s => s.DeletedAt)
                .ToListAsync();
        }

        public async Task<DeletedSnapshot?> GetByIdAsync(int id)
        {
            return await _context.DeletedSnapshots.FindAsync(id);
        }

        public async Task<List<DeletedSnapshot>> GetByEntityTypeAndIdAsync(string entityType, string entityId)
        {
            return await _context.DeletedSnapshots
                .Where(s => s.EntityType == entityType && s.EntityId == entityId)
                .OrderByDescending(s => s.DeletedAt)
                .ToListAsync();
        }
    }
}
