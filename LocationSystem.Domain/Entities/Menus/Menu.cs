using LocationSystem.Domain.Entities.Interfacies;
using LocationSystem.Domain.Entities.UserRolePermissions;
using System.ComponentModel.DataAnnotations;

namespace LocationSystem.Domain.Entities.Menus
{
    public class Menu : IEntityVisiable
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string Icon { get; private set; }
        public int Order { get; private set; }
        public Guid? ParentId { get; private set; }
        public Menu? Parent { get; private set; }
        public List<Menu> Children { get; private set; }
        public List<PermissionMenu> PermissionMenus { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        [Timestamp]
        public byte[] Version { get; private set; }
        public bool IsVisiable { get; set; }

        public Menu(string name, string path, string icon, int order, Guid? parentId = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Path = path;
            Icon = icon;
            Order = order;
            ParentId = parentId;
            Children = new List<Menu>();
            PermissionMenus = new List<PermissionMenu>();
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string path, string icon, int order, Guid? parentId = null)
        {
            Name = name;
            Path = path;
            Icon = icon;
            Order = order;
            ParentId = parentId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddChild(Menu child)
        {
            child.ParentId = Id;
            child.Parent = this;
            Children.Add(child);
        }
    }
}
