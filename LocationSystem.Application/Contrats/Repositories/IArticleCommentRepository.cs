using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IArticleCommentRepository : IRepository<ArticleComment>
    {
        Task<IEnumerable<ArticleComment>> GetByArticleIdsAsync(IEnumerable<Guid> articleIds);
    }
}
