using LocationSystem.Application.Dtos.Roles;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<RoleDto>
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public List<Guid>? PermissionIds { get; set; }
    }
}