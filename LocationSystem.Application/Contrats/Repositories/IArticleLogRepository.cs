using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IArticleLogRepository : IRepository<ArticleLog>
    {
        Task<Dictionary<Guid, List<ArticleLogGraphqLDto>>> GetArticleLogByIds(IReadOnlyList<Guid> ids);
    }
}
