using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommand : IRequset<PermissionDto>
    {
        public Guid PermissionId { get; set; }
        public UpdatePermissionDto PermissionDto { get; set; } = null!;
    }
}