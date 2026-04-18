using LocationSystem.Application.Dtos.Permissions;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommand : IRequest<PermissionDto>
    {
        public CreatePermissionDto PermissionDto { get; set; } = null!;
    }
}