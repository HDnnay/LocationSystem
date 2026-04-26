using LocationSystem.Application.GrapqLDTOs.Users;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Queries.GetUserByIds
{
    public class GetUserByIdsQuery : IRequest<Dictionary<Guid, UserGraphqLDto>>
    {
        public GetUserByIdsQuery(IReadOnlyList<Guid>? ids)
        {
            Ids = ids;
        }
        public IReadOnlyList<Guid>? Ids { get; set; }
    }
}
