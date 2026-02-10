using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Dtos
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? ParentId { get; set; }
        public List<RoleDto>? Roles { get; set; }
        public List<PermissionDto>? ChildPermissions { get; set; }
    }

    public class CreatePermissionDto
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
    }

    public class UpdatePermissionDto
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
    }
}