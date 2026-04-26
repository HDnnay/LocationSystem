using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleTagRepository : Repository<ArticleTag>, IArticleTagRepository
    {
        public ArticleTagRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Dictionary<Guid, List<ArticleTagGraphqLDto>>> GetArticleTagByIds(IReadOnlyList<Guid> ids)
        {
            throw new NotImplementedException();
        }
    }
}
