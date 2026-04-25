using GreenDonut;
using LocationSystem.Application.Features.Menus.Queries.GetMenusByIds;
using LocationSystem.Application.GrapqLDTOs.Menus;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.DataLoaders
{
    public class MenuDataLoader : BatchDataLoader<Guid, MenuGraphqLDto>
    {
        private readonly IMediator _mediator;

        public MenuDataLoader(
            IMediator mediator,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _mediator = mediator;
        }

        protected override async Task<IReadOnlyDictionary<Guid, MenuGraphqLDto>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            var query = new GetMenusByIdsQuery { Ids = keys };
            var result = await _mediator.Send(query);
            return result;
        }
    }
}