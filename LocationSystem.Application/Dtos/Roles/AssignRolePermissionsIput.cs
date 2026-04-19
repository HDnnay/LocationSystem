namespace LocationSystem.Application.Dtos.Roles
{
    public class AssignRolePermissionsIput
    {
        public Guid Id { get; set; }
        public List<Guid>? Permissions { get; set; }
    }
}
