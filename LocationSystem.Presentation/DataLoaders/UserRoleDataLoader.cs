using LocationSystem.Application.Features.Roles.Queries.GetRolesByUser;
using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.DataLoaders
{
    public static class UserRoleDataLoader
    {
        [DataLoader]
        public static Task<Dictionary<Guid, List<RoleGraphqLDto>>> GetRolesByUserAsync(
            IReadOnlyList<Guid> ids,
            [Service] IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var query = new GetRolesByUserQuery() { Ids = ids };
            return mediator.Send(query);
        }
    }
}
