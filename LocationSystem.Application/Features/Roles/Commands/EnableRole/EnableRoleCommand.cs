using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Commands.EnableRole
{
    public class EnableRoleCommand : IRequest<bool>
    {
        public Guid RoleId { get; set; }
    }
}
