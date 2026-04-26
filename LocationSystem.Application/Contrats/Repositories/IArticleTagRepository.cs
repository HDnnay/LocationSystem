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
    }
}
