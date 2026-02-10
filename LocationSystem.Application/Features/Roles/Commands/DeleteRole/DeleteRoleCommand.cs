using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommand : IRequset<bool>
    {
        public Guid RoleId { get; set; }
    }
}