using LocationSystem.Application.Dtos.Users;
using Mapster;
using System.ComponentModel;

namespace LocationSystem.Application.GrapqLDTOs
{
    [AdaptTo(typeof(UserDto))]

    public class UserGrapqLDto
    {
        [Description("用户Id")]
        public Guid Id { get; set; }
        [Description("用户名字")]

        public string Name { get; set; }
        [Description("邮箱")]

        public string Email { get; set; }
        [Description("类型")]

        public string UserType { get; set; }
        [Description("是否禁用")]

        public bool IsDisabled { get; set; }
        [Description("创建时间")]

        public DateTime CreateTime { get; set; }
        [Description("删除时间")]

        public DateTime? DeleteTime { get; set; }
        [Description("是否删除")]

        public bool IsDelete { get; set; }
    }
}
