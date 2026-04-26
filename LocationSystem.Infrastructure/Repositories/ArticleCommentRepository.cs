using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleCommentRepository : Repository<ArticleComment>
    {
        public ArticleCommentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
