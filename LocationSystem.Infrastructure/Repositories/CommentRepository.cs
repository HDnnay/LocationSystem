using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.Articles;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class CommentRepository : Repository<ArticleComment>, ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleComment>> GetByArticleIdsAsync(IEnumerable<Guid> articleIds)
        {
            return await _context.ArticleComments
                .Where(c => articleIds.Contains(c.ArticleId))
                .ToListAsync();
        }
    }
}
