using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleImageRepository : Repository<ArticleImage>, IArticleImageRepository
    {
        public ArticleImageRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Dictionary<Guid, List<ArticleImageGraphqLDto>>> GetArticleImagesByIdsAsync(IReadOnlyList<Guid> ids)
        {
            throw new NotImplementedException();
        }
    }
}
