using LocationSystem.Domain.Entities.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<IEnumerable<Tag>> GetByIdsAsync(IEnumerable<Guid> ids);
    }
}
