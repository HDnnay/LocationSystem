using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequest<RoleDto>
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public List<Guid>? PermissionIds { get; set; }
    }
}