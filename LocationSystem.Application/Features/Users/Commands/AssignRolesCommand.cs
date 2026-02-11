using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands
{
    public class AssignRolesCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public IEnumerable<Guid> RoleIds { get; set; }
    }
}
