using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleCommentRepository : Repository<ArticleComment>, IArticleCommentRepository
    {
        private readonly AppDbContext _context;

        public ArticleCommentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Dictionary<Guid, ArticleCommentGraphqLDto>> GetByArticleIdsAsync(IEnumerable<Guid> articleIds)
        {
            return await _context.ArticleComments
                .Where(c => articleIds.Contains(c.ArticleId)).ProjectToType<ArticleCommentGraphqLDto>()
                .ToDictionaryAsync(t => t.Id);
        }
    }
}
