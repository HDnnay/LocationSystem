using LocationSystem.Application.Dtos.Roles;

namespace LocationSystem.Application.Dtos.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? DeleteTime { get; set; }
        public bool IsDelete { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
