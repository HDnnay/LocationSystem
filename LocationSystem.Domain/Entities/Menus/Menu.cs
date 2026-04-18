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
        public int Level { get; private set; }
        public bool IsBackEnd { get; private set; }
        public Guid? ParentId { get; private set; }
        public virtual Menu? Parent { get; private set; }
        public List<Menu> Children { get; private set; }
        public List<PermissionMenu> PermissionMenus { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        [Timestamp]
        public byte[] Version { get; private set; }
        public bool IsVisiable { get; set; }

        public DateTime CreateTime { get; private set; }

        public Menu(string name, string path, string icon, int order, int level, Guid? parentId = null, bool isBackEnd = false)
        {
            Id = Guid.NewGuid();
            Name = name;
            Path = path;
            Icon = icon;
            Order = order;
            Level = level;
            ParentId = parentId;
            IsBackEnd = isBackEnd;
            Children = new List<Menu>();
            PermissionMenus = new List<PermissionMenu>();
            CreateTime = DateTime.Now;
        }

        public void Update(string name, string path, string icon, int order, int level, Guid? parentId = null, bool isBackEnd = false)
        {
            Name = name;
            Path = path;
            Icon = icon;
            Order = order;
            Level = level;
            ParentId = parentId;
            IsBackEnd = isBackEnd;
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
