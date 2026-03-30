using GreenDonut;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.UserRolePermissions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocationSystem.Api.GraphQL.DataLoaders
{
    public class RoleDataLoader : BatchDataLoader<Guid, Role>
    {
        private readonly IServiceProvider _serviceProvider;

        public RoleDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider)
            : base(batchScheduler, new DataLoaderOptions())
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<IReadOnlyDictionary<Guid, Role>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var roleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();

            // 使用批量查询获取所有角色
            var roles = await roleRepository.GetRolesWithPermissionsByIdsAsync(keys.ToList());
            
            // 将结果映射到字典
            var result = roles.ToDictionary(role => role.Id);

            return result;
        }
    }
}
