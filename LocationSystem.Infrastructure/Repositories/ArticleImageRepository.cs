using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleImageRepository : Repository<ArticleImage>, IArticleImageRepository
    {
        public ArticleImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
