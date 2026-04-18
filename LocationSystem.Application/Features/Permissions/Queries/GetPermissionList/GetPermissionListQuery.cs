using LocationSystem.Application.Dtos.Permissions;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionList
{
    public class GetPermissionListQuery :PageRequest, IRequest<PageResult<PermissionDto>>
    {
    }
}