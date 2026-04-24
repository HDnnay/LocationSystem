using LocationSystem.Application.Features.Users.Queries.GetUserByIds;
using LocationSystem.Application.GrapqLDTOs;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Presentation.DataLoaders
{
    public static class UserDataLoader
    {
        [DataLoader]
        public static async Task<Dictionary<Guid, UserGrapqLDto>> GetUsersAsync(
            IReadOnlyList<Guid> ids,
            [Service] IMediator mediator,
            CancellationToken cancellationToken)
        {
            var query = new GetUserByIdsQuery(ids);
            var model = await mediator.Send(query);
            return model.ToDictionary(t => t.Key, t => t.Adapt<UserGrapqLDto>());
        }
    }
}
