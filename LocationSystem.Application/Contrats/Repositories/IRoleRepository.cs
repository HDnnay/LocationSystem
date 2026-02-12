using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<Role?> GetByCodeAsync(string code);
        Task<Role?> GetRoleWithPermissionsAsync(Guid id);
        Task<IEnumerable<Role>> GetRolesWithPermissionsAsync();
        Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId);
    }
}