using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionTreeWithCheckStatus
{
    public class GetPermissionTreeWithCheckStatusQuery : IRequest<List<PermissionTreeDto>>
    {
        public Guid? RoleId { get; set; }
    }
}