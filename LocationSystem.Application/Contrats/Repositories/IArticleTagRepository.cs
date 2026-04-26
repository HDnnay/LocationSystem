using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IArticleTagRepository : IRepository<ArticleTag>
    {
        Task<IEnumerable<ArticleTag>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<ILookup<Guid, ArticleTagGraphqLDto>> GetTagsByArticleIdsAsync(IReadOnlyList<Guid> articleIds, CancellationToken cancellationToken = default);
    }
}