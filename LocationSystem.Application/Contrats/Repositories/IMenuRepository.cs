using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {
        Task<List<Menu>> GetMenusByPermissionIdsAsync(List<Guid> permissionIds);
        Task<List<Menu>> GetAllMenusAsync();
        Task<Menu?> GetMenuByIdAsync(Guid id);
        Task<Menu> CreateMenuAsync(Menu menu);
        Task UpdateMenuAsync(Menu menu);
        Task DeleteMenuAsync(Guid id);
    }
}
