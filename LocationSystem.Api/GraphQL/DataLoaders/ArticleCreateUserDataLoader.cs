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
    public class ArticleCreateUserDataLoader : BatchDataLoader<Guid, User>
    {
        private readonly IServiceProvider _serviceProvider;

        public ArticleCreateUserDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider)
            : base(batchScheduler, new DataLoaderOptions())
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<IReadOnlyDictionary<Guid, User>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var result = new Dictionary<Guid, User>();

            foreach (var id in keys)
            {
                var user = await userRepository.GetByIdAsync(id);
                if (user != null)
                {
                    result[id] = user;
                }
            }

            return result;
        }
    }
}
