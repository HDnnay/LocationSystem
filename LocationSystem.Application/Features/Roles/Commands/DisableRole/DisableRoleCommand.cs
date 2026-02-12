using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Commands.DisableRole
{
    public class DisableRoleCommand : IRequest<bool>
    {
        public Guid RoleId { get; set; }
    }
}
