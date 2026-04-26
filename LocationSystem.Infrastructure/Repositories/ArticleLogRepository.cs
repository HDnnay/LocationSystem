using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleLogRepository : Repository<ArticleComment>, IArticleLogRepository
    {
        public ArticleLogRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Dictionary<Guid, List<ArticleLogGraphqLDto>>> GetArticleLogByIds(IReadOnlyList<Guid> ids)
        {
            throw new NotImplementedException();
        }
    }
}
