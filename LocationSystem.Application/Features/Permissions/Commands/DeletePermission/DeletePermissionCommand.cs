using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Permissions.Commands.DeletePermission
{
    public class DeletePermissionCommand : IRequset<bool>
    {
        public Guid PermissionId { get; set; }
    }
}