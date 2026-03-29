using HotChocolate;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.UserRolePermissions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocationSystem.Api.GraphQL.DataLoaders
{
    public class RolePermissionsDataLoader : BatchDataLoader<Guid, List<Permission>>
    {
        private readonly IServiceProvider _serviceProvider;

        public RolePermissionsDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider) : base(batchScheduler, GetOptions())
        {
            _serviceProvider = serviceProvider;
        }

        private static DataLoaderOptions GetOptions()
        {
            return new DataLoaderOptions();
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<Permission>>> LoadBatchAsync(IReadOnlyList<Guid> roleIds, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var roleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
                var roles = await roleRepository.GetRolesWithPermissionsByIdsAsync(roleIds.ToList());

                var result = new Dictionary<Guid, List<Permission>>();
                foreach (var role in roles)
                {
                    result[role.Id] = role.Permissions.ToList();
                }

                // 确保所有请求的角色都有结果
                foreach (var roleId in roleIds)
                {
                    if (!result.ContainsKey(roleId))
                    {
                        result[roleId] = new List<Permission>();
                    }
                }

                return result;
            }
        }
    }
}