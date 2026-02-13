using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Dtos
{
    public class PermissionTreeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsCheck { get; set; }
        public List<PermissionTreeDto> ChildPermissions { get; set; } = new List<PermissionTreeDto>();
    }
}