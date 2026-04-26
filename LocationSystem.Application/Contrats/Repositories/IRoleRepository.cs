using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Domain.Entities.UserRolePermissions;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<Role?> GetByCodeAsync(string code);
        Task<Role?> GetRoleWithPermissionsAsync(Guid id);
        Task<IEnumerable<Role>> GetRolesWithPermissionsAsync();
        Task<List<Role>> GetRolesWithPermissionsByIdsAsync(List<Guid> roleIds);
        Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId);
        Task<Dictionary<Guid, List<RoleGraphqLDto>>> GetRolesByUserIdsAsync(IReadOnlyList<Guid> userIds);
        Task<Dictionary<Guid, RoleGraphqLDto>> GetRoleByIds(IReadOnlyList<Guid> ids, CancellationToken cts = default);
    }
}