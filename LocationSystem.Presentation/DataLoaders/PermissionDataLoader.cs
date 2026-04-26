using LocationSystem.Application.Features.Permissions.Queries.GetPermissionsByIds;
using LocationSystem.Application.GrapqLDTOs.Permissons;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation.DataLoaders
{
    public class PermissionDataLoader : BatchDataLoader<Guid, PermissionGraphqLDto>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PermissionDataLoader(
            IServiceScopeFactory scopeFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, PermissionGraphqLDto>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            // 创建临时作用域，确保 DbContext 隔离
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var query = new GetPermissionsByIdsQuery { Ids = keys };
            var result = await mediator.Send(query);
            return result;
        }
    }
}