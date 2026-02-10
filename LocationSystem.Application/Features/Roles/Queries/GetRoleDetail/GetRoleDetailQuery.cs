using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Roles.Queries.GetRoleDetail
{
    public class GetRoleDetailQuery : IRequest<RoleDto>
    {
        public Guid RoleId { get; set; }
    }
}