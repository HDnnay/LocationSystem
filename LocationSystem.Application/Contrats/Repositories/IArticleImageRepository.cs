using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IArticleImageRepository : IRepository<ArticleImage>
    {
        Task<Dictionary<Guid, List<ArticleImageGraphqLDto>>> GetArticleImagesByIdsAsync(IReadOnlyList<Guid> ids);
    }
}
