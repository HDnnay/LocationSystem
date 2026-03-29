using LocationSystem.Domain.Entities.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<Article?> GetByIdAsync(Guid id, bool includeTags = false);
        IQueryable<Article> GetAllQueryable();
    }
}
