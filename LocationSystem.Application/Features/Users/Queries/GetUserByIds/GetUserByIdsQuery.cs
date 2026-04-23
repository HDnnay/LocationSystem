using LocationSystem.Application.Dtos.Users;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Queries.GetUserByIds
{
    public class GetUserByIdsQuery : IRequest<Dictionary<Guid, UserDto>>
    {
        public GetUserByIdsQuery(IReadOnlyList<Guid>? ids)
        {
            Ids = ids;
        }
        public IReadOnlyList<Guid>? Ids { get; set; }
    }
}
