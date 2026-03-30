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
    public class UserDataLoader : BatchDataLoader<Guid, User>
    {
        private readonly IServiceProvider _serviceProvider;

        public UserDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider)
            : base(batchScheduler, new DataLoaderOptions())
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<IReadOnlyDictionary<Guid, User>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            // 使用批量查询获取所有用户
            var users = await userRepository.GetByIdsWithRolesAsync(keys.ToList());
            
            // 将结果映射到字典
            var result = users.ToDictionary(user => user.Id);

            return result;
        }
    }
}
