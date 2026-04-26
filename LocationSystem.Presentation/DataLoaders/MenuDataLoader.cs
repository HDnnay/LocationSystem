using LocationSystem.Application.Features.Menus.Queries.GetMenusByIds;
using LocationSystem.Application.GrapqLDTOs.Menus;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation.DataLoaders
{
    public class MenuDataLoader : BatchDataLoader<Guid, MenuGraphqLDto>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public MenuDataLoader(
            IServiceScopeFactory scopeFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, MenuGraphqLDto>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            // 创建临时作用域，确保 DbContext 隔离
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var query = new GetMenusByIdsQuery { Ids = keys };
            var result = await mediator.Send(query);
            return result;
        }
    }
}