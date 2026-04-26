using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleImageRepository : Repository<ArticleImage>
    {
        public ArticleImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
