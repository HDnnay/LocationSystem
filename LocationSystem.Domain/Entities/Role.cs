using LocationSystem.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.Entities
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class Role
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Code { get; private set; } = null!;
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // 导航属性
        public ICollection<Permission> Permissions { get; private set; } = new List<Permission>();
        public ICollection<User> Users { get; private set; } = new List<User>();

        private Role() { }

        public Role(string name, string code, string? description = null)
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
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string code, string? description = null)
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
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddPermission(Permission permission)
        {
            if (!Permissions.Contains(permission))
            {
                Permissions.Add(permission);
            }
        }

        public void RemovePermission(Permission permission)
        {
            Permissions.Remove(permission);
        }

        public void ClearPermissions()
        {
            Permissions.Clear();
        }
    }
}