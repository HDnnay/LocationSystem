using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities.Common;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<Permission?> GetByNameAsync(string name);
        Task<Permission?> GetByCodeAsync(string code);
        Task<Permission?> GetPermissionWithRolesAsync(Guid id);
        Task<IEnumerable<Permission>> GetPermissionsWithRolesAsync();
        Task<IEnumerable<Permission>> GetPermissionTreeAsync();
        Task<List<PermissionTreeDto>> GetPermissionTreeDtosAsync();
        Task<List<PermissionTreeDto>> GetPermissionTreeWithCheckStatusAsync(Guid? roleId);
        Task<Permission?> GetPermissionWithChildrenAsync(Guid id);
        Task<Dictionary<int,IEnumerable<Permission>>> GetPermissionsPage(PageRequest pageRequest);

    }
}