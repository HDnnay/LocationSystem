using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Queries.GetRoleByIds
{
    public class GetRoleByIdsQueryHandler : IRequestHandler<GetRoleByIdsQuery, Dictionary<Guid, RoleGraphqLDto>>
    {
        public Task<Dictionary<Guid, RoleGraphqLDto>> Handle(GetRoleByIdsQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
