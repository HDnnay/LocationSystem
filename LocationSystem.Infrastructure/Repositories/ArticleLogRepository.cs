using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleLogRepository : Repository<ArticleComment>
    {
        public ArticleLogRepository(AppDbContext context) : base(context)
        {
        }
    }
}
