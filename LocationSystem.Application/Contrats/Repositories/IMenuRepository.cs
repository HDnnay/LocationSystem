using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {
        Task<List<Menu>> GetMenusByPermissionIdsAsync(List<Guid> permissionIds);
    }
}
