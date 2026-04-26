using LocationSystem.Application.GrapqLDTOs.Articles;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IArticleLogRepository
    {
        Task<Dictionary<Guid, List<ArticleLogGraphqLDto>>> GetArticleLogByIds(IReadOnlyList<Guid> ids);
    }
}
