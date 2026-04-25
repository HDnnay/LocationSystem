using GreenDonut;
using LocationSystem.Application.Features.Roles.Queries.GetRoleByIds;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.DataLoaders
{
    public class RoleDataLoader : BatchDataLoader<Guid, RoleGraphqLDto>
    {
        private readonly IMediator _mediator;

        public RoleDataLoader(
            IMediator mediator,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _mediator = mediator;
        }

        protected override async Task<IReadOnlyDictionary<Guid, RoleGraphqLDto>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            var query = new GetRoleByIdsQuery(keys);
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
