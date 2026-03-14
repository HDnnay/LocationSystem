using System;
using System.Collections.Generic;

namespace LocationSystem.Application.Features.Permissions.Models
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? ParentId { get; set; }
        public List<PermissionDto> ChildPermissions { get; set; }
    }
}