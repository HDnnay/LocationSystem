using LocationSystem.Application.Dtos.Roles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Commands.AssignRolePermissions
{
    public class AssignRolePermissionCommandHandler : IRequestHandler<AssignRolePermissionCommand, RoleDto>
    {
        public AssignRolePermissionCommandHandler()
        {

        }
        public Task<RoleDto> Handle(AssignRolePermissionCommand request)
        {
            throw new NotImplementedException();
        }
    }
}
