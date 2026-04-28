using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleLogRepository : Repository<ArticleLog>, IArticleLogRepository
    {
        private readonly AppDbContext _context;
        public ArticleLogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Dictionary<Guid, List<ArticleLogGraphqLDto>>> GetArticleLogByIds(IReadOnlyList<Guid> ids)
        {
            throw new NotImplementedException();
        }
    }
}
