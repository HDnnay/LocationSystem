using LocationSystem.Application.Features.Menus.Queries.GetAllMenus;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {

        Task<IEnumerable<Menu>> GetMenuPage(GetAllMenusQuery query);
        Task<int> GetTotalCount();
    }
}
