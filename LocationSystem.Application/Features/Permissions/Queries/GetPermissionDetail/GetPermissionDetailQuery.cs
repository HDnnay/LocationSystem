using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Permissions.Queries.GetPermissionDetail
{
    public class GetPermissionDetailQuery : IRequest<PermissionDto>
    {
        public Guid PermissionId { get; set; }
    }
}