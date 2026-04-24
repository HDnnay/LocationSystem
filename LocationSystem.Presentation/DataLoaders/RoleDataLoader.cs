using LocationSystem.Application.Features.Roles.Queries.GetRoleByIds;
using LocationSystem.Application.Features.Roles.Queries.GetRolesByUser;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.DataLoaders
{

    public static class RoleDataLoader
    {
        [DataLoader]

        public static async Task<Dictionary<Guid, RoleGraphqLDto>> GetRolesAsync(IReadOnlyList<Guid> ids,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            var query = new GetRoleByIdsQuery(ids);
            var result = await mediator.Send(query);
            return result;
        }
        [DataLoader]
        public static async Task<Dictionary<Guid, List<RoleGraphqLDto>>> GetRolesByUserAsync(IReadOnlyList<Guid> ids, [Service] IMediator mediator, CancellationToken cts = default)
        {
            var query = new GetRolesByUserQuery() { Ids = ids };
            var result = await mediator.Send(query);
            return result;
        }
    }
}
