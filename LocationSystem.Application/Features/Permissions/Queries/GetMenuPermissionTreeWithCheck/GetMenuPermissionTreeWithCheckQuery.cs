using LocationSystem.Application.Dtos.Permissions;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Permissions.Queries.GetMenuPermissionTreeWithCheck
{
    public class GetMenuPermissionTreeWithCheckQuery: IRequest<List<PermissionTreeDto>>
    {
        public Guid? MenuId { get; set; }
    }
}
