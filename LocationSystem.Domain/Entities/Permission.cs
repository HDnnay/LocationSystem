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

        private Permission() { }

        public Permission(string name, string code, string? description = null, Guid? parentId = null)
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
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string code, string? description = null, Guid? parentId = null)
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
            UpdatedAt = DateTime.UtcNow;
        }
    }
}