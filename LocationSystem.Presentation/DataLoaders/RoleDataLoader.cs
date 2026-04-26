using LocationSystem.Application.Features.Roles.Queries.GetRoleByIds;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation.DataLoaders
{
    public class RoleDataLoader : BatchDataLoader<Guid, RoleGraphqLDto>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RoleDataLoader(
            IServiceScopeFactory scopeFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, RoleGraphqLDto>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            // 创建临时作用域，确保 DbContext 隔离
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var query = new GetRoleByIdsQuery(keys);
            var result = await mediator.Send(query);
            return result;
        }
    }
}
