namespace LocationSystem.Application.Features.Roles.Commands.AssignRolePermissions
{
    public class AssignRolePermissionCommand : LocationSystem.Application.Utilities.IRequest<LocationSystem.Application.Dtos.Roles.RoleDto>
    {
        public Guid RoleId { get; set; }
        public List<Guid>? Permissions { get; set; }
    }
}
