using GreenDonut;
using LocationSystem.Application.Features.Roles.Queries.GetRolesByUser;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.DataLoaders
{
    public class UserRoleDataLoader : BatchDataLoader<Guid, List<RoleGraphqLDto>>
    {
        private readonly IMediator _mediator;

        public UserRoleDataLoader(
            IMediator mediator,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _mediator = mediator;
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<RoleGraphqLDto>>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            var query = new GetRolesByUserQuery { Ids = keys };
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
