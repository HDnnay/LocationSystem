using System.ComponentModel;

namespace LocationSystem.Application.GrapqLDTOs.Roles
{
    public class RoleGraphqLDto
    {
        [Description("角色Id")]

        public Guid Id { get; set; }
        [Description("角色名称")]

        public string Name { get; set; } = null!;
        [Description("角色代码")]

        public string Code { get; set; } = null!;
        [Description("角色描述")]

        public string? Description { get; set; }
        [Description("是否禁用")]

        public bool IsDisabled { get; set; }
        [Description("创建时间")]

        public DateTime CreatedAt { get; set; }
        [Description("更新时间")]

        public DateTime? UpdatedAt { get; set; }
    }
}
