using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequset<RoleDto>
    {
        public CreateRoleDto RoleDto { get; set; } = null!;
    }
}