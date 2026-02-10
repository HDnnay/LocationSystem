using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionList
{
    public class GetPermissionListQuery : IRequest<List<PermissionDto>>
    {
    }
}