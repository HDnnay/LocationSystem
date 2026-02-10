using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<PermissionDto>? Permissions { get; set; }
    }

    public class CreateRoleDto
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public List<Guid>? PermissionIds { get; set; }
    }

    public class UpdateRoleDto
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public List<Guid>? PermissionIds { get; set; }
    }
}