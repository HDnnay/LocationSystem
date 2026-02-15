using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IPermissionMenuRepository : IRepository<PermissionMenu>
    {
        Task<IEnumerable<PermissionMenu>> GetByMenuIdAsync(Guid menuId);
    }
}