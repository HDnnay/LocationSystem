using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleTagRepository : Repository<ArticleTag>, IArticleTagRepository
    {
        public ArticleTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
