using LocationSystem.Application.Features.Menus.Queries.GetAllMenus;
using LocationSystem.Application.GrapqLDTOs.Menus;
using LocationSystem.Domain.Entities.Menus;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {

        Task<IEnumerable<Menu>> GetMenuPage(GetAllMenusQuery query);
        Task<int> GetTotalCount();
        Task<IEnumerable<Menu>> GetMenuTreeAsync();
        Task<IEnumerable<Menu>> GetAllWithPermissionsAsync();
        Task<Menu?> GetByIdWithPermissionsAsync(Guid id);
        Task<IEnumerable<Menu>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<List<Menu>> GetByIdsWithPermissionsAsync(List<Guid> menuIds);
        Task<Dictionary<Guid, MenuGraphqLDto>> GetMenusByIdsAsync(IReadOnlyList<Guid> menuIds);
    }
}
