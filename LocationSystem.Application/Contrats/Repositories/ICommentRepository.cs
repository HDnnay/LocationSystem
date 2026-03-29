using LocationSystem.Domain.Entities.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface ICommentRepository : IRepository<ArticleComment>
    {
        Task<IEnumerable<ArticleComment>> GetByArticleIdsAsync(IEnumerable<Guid> articleIds);
    }
}
