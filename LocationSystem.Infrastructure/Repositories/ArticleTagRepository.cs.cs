using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.Articles;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class TagRepository : Repository<ArticleTag>, IArticleTagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleTag>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.ArticleTags
                .Where(t => ids.Contains(t.Id))
                .ToListAsync();
        }
    }
}
