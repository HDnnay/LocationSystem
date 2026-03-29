using LocationSystem.Application.Contrats;
using LocationSystem.Domain.Entities.Articles;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppDbContext _context;

        public ArticleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Article> GetByIdAsync(Guid id, bool includeRelated = false)
        {
            var query = _context.Articles.AsQueryable();

            if (includeRelated)
            {
                query = query
                    .Include(a => a.Tags)
                    .Include(a => a.Comments)
                    .Include(a => a.CreateUser);
            }

            return await query.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Article>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.Articles
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
        }

        public async Task<List<Article>> GetAll()
        {
            return await _context.Articles.ToListAsync();
        }

        public IQueryable<Article> GetAllQueryable()
        {
            return _context.Articles.AsQueryable();
        }

        public async Task AddAsync(Article article)
        {
            await _context.Articles.AddAsync(article);
        }

        public async Task UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
        }

        public async Task DeleteAsync(Article article)
        {
            _context.Articles.Remove(article);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tag> GetByIdAsync(Guid id)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.Tags
                .Where(t => ids.Contains(t.Id))
                .ToListAsync();
        }

        public async Task<List<Tag>> GetAll()
        {
            return await _context.Tags.ToListAsync();
        }
    }
}