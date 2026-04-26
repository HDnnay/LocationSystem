using LocationSystem.Application.Features.Users.Queries.GetUserByIds;
using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation.DataLoaders
{
    public class UserDataLoader : BatchDataLoader<Guid, UserGraphqLDto>
    {
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _scopeFactory; // 注入作用域工厂
        public UserDataLoader(
            IServiceScopeFactory scopeFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, UserGraphqLDto>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)

        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var query = new GetUserByIdsQuery(keys);
            var model = await mediator.Send(query);
            return model;
        }
    }
}
