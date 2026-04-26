using LocationSystem.Application.GrapqLDTOs.Articles;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IArticleTagRepository
    {
        Task<Dictionary<Guid, List<ArticleTagGraphqLDto>>> GetArticleTagByIds(IReadOnlyList<Guid> ids);

    }
}
