using LocationSystem.Application.Features.Permissions.Queries.GetPermissionsByIds;
using LocationSystem.Application.GrapqLDTOs.Permissons;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.DataLoaders
{
    public class PermissionDataLoader : BatchDataLoader<Guid, PermissionGraphqLDto>
    {
        private readonly IMediator _mediator;

        public PermissionDataLoader(
            IMediator mediator,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _mediator = mediator;
        }

        protected override async Task<IReadOnlyDictionary<Guid, PermissionGraphqLDto>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            var query = new GetPermissionsByIdsQuery { Ids = keys };
            var result = await _mediator.Send(query);
            return result;
        }
    }
}