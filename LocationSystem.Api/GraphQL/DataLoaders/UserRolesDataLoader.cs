using HotChocolate;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.UserRolePermissions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocationSystem.Api.GraphQL.DataLoaders
{
    public class UserRolesDataLoader : BatchDataLoader<Guid, List<Role>>
    {
        private readonly IServiceProvider _serviceProvider;

        public UserRolesDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider) : base(batchScheduler, GetOptions())
        {
            _serviceProvider = serviceProvider;
        }

        private static DataLoaderOptions GetOptions()
        {
            return new DataLoaderOptions();
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<Role>>> LoadBatchAsync(IReadOnlyList<Guid> userIds, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var users = await userRepository.GetByIdsWithRolesAsync(userIds.ToList());

                var result = new Dictionary<Guid, List<Role>>();
                foreach (var user in users)
                {
                    result[user.Id] = user.Roles.ToList();
                }

                // 确保所有请求的用户都有结果
                foreach (var userId in userIds)
                {
                    if (!result.ContainsKey(userId))
                    {
                        result[userId] = new List<Role>();
                    }
                }

                return result;
            }
        }
    }
}