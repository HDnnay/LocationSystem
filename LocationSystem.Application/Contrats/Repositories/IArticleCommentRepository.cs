using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IArticleCommentRepository : IRepository<ArticleComment>
    {
        Task<IEnumerable<ArticleCommentGraphqLDto>> GetByArticleIdsAsync(IEnumerable<Guid> articleIds);
    }
}
