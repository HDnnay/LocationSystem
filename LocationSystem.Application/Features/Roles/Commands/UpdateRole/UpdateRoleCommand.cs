using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequset<RoleDto>
    {
        public Guid RoleId { get; set; }
        public UpdateRoleDto RoleDto { get; set; } = null!;
    }
}