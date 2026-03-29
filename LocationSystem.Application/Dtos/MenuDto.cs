using System;
using System.Collections.Generic;

namespace LocationSystem.Application.Dtos
{
    public class MenuDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<MenuDto>? ChildMenus { get; set; }
    }

    public class PermissionMenuDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsMenu { get; set; }
        public string MenuPath { get; set; }
        public string MenuIcon { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<PermissionDto> Permissions { get; set; }
        public List<PermissionMenuDto> Children { get; set; }
    }
}