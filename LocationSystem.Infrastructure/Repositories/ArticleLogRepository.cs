using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleLogRepository : Repository<ArticleComment>, IArticleLogRepository
    {
        public ArticleLogRepository(AppDbContext context) : base(context)
        {
        }
    }
}
