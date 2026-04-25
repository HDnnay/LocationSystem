using GreenDonut;
using LocationSystem.Application.Features.Users.Queries.GetUserByIds;
using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Presentation.DataLoaders
{
    public class UserDataLoader : BatchDataLoader<Guid, UserGraphqLDto>
    {
        private readonly IMediator _mediator;

        public UserDataLoader(
            IMediator mediator,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _mediator = mediator;
        }

        protected override async Task<IReadOnlyDictionary<Guid, UserGraphqLDto>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            var query = new GetUserByIdsQuery(keys);
            var model = await _mediator.Send(query);
            return model.ToDictionary(t => t.Key, t => t.Adapt<UserGraphqLDto>());
        }
    }
}
