using LocationSystem.Application.GrapqLDTOs.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Queries.GetRolesByUser
{
    internal class GetRolesByUserQueryHandler : IRequestHandler<GetRolesByUserQuery, ILookup<Guid, RoleGraphqLDto>>
    {
        public Task<ILookup<Guid, RoleGraphqLDto>> Handle(GetRolesByUserQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
