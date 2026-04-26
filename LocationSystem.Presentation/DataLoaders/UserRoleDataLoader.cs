using LocationSystem.Application.Features.Roles.Queries.GetRolesByUser;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation.DataLoaders
{
    public class UserRoleDataLoader : BatchDataLoader<Guid, List<RoleGraphqLDto>>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UserRoleDataLoader(
            IServiceScopeFactory scopeFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<RoleGraphqLDto>>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            // 创建临时作用域，确保 DbContext 隔离
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var query = new GetRolesByUserQuery { Ids = keys };
            var result = await mediator.Send(query);
            return result;
        }
    }
}
