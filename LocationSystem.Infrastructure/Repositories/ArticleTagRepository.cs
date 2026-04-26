using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleTagRepository : Repository<ArticleTag>
    {
        public ArticleTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
