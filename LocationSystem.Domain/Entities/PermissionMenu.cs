namespace LocationSystem.Domain.Entities
{
    public class PermissionMenu
    {
        public Guid Id { get; private set; }
        public Guid PermissionId { get; private set; }
        public Permission Permission { get; private set; }
        public Guid MenuId { get; private set; }
        public Menu Menu { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public PermissionMenu(Permission permission, Menu menu)
        {
            Id = Guid.NewGuid();
            PermissionId = permission.Id;
            Permission = permission;
            MenuId = menu.Id;
            Menu = menu;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
