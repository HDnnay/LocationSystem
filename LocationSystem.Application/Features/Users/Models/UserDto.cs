namespace LocationSystem.Application.Features.Users.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public bool IsDisabled { get; set; }
        public List<RoleDto> Roles { get; set; }
    }

    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }

    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
