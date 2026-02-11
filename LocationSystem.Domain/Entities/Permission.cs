using LocationSystem.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    /// <summary>
    /// 权限实体
    /// </summary>
    public class Permission
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Code { get; private set; } = null!;
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // 导航属性
        public ICollection<Role> Roles { get; private set; } = new List<Role>();
        
        // 父权限ID
        public Guid? ParentId { get; private set; }
        // 父权限导航属性
        public Permission? Parent { get; private set; }
        // 子权限导航属性
        public ICollection<Permission> ChildPermissions { get; private set; } = new List<Permission>();

        // 菜单相关属性
        public bool IsMenu { get; private set; }
        public string? MenuPath { get; private set; }
        public string? MenuIcon { get; private set; }
        public int Order { get; private set; }

        private Permission() { }

        public Permission(string name, string code, string? description = null, Guid? parentId = null, bool isMenu = false, string? menuPath = null, string? menuIcon = null, int order = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinessRuleException($"{nameof(name)}不能为空");
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BussinessRuleException($"{nameof(code)}不能为空");
            }

            Id = Guid.NewGuid();
            Name = name;
            Code = code;
            Description = description;
            ParentId = parentId;
            IsMenu = isMenu;
            MenuPath = menuPath;
            MenuIcon = menuIcon;
            Order = order;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string code, string? description = null, Guid? parentId = null, bool? isMenu = null, string? menuPath = null, string? menuIcon = null, int? order = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinessRuleException($"{nameof(name)}不能为空");
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BussinessRuleException($"{nameof(code)}不能为空");
            }

            Name = name;
            Code = code;
            Description = description;
            ParentId = parentId;
            IsMenu = isMenu ?? IsMenu;
            MenuPath = menuPath ?? MenuPath;
            MenuIcon = menuIcon ?? MenuIcon;
            Order = order ?? Order;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}