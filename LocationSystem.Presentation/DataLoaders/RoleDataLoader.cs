using LocationSystem.Application.Features.Roles.Queries.GetRoleByIds;
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

    }
}
