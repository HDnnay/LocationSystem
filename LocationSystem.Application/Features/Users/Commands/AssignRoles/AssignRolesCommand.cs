using LocationSystem.Application.Utilities;
using System.Collections.Generic;

namespace LocationSystem.Application.Features.Users.Commands.AssignRoles
{
    public class AssignRolesCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public List<Guid> RoleIds { get; set; }
    }
}
