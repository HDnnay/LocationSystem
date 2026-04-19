using MediatR;

namespace LocationSystem.Application.Features.Roles.Commands.AssignRolePermissions
{
    public class AssignRolePermissionCommand : IRequest
    {
        public Guid RoleId { get; set; }
        public List<Guid>? Permissions { get; set; }
    }
}
