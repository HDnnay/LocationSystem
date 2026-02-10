using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<Permission?> GetByNameAsync(string name);
        Task<Permission?> GetByCodeAsync(string code);
        Task<Permission?> GetPermissionWithRolesAsync(Guid id);
        Task<IEnumerable<Permission>> GetPermissionsWithRolesAsync();
        Task<IEnumerable<Permission>> GetPermissionTreeAsync();
        Task<Permission?> GetPermissionWithChildrenAsync(Guid id);
    }
}