using LocationSystem.Application.GrapqLDTOs.Articles;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IArticleImageRepository
    {
        Task<Dictionary<Guid, List<ArticleImageGraphqLDto>>> GetArticleImagesByIdsAsync(IReadOnlyList<Guid> ids);
    }
}
