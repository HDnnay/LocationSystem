using LocationSystem.Application.Dtos.Roles;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Roles.Queries.GetRoleList
{
    public class GetRoleListQuery : IRequest<List<RoleDto>>
    {
    }
}