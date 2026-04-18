using System.ComponentModel;

namespace LocationSystem.Domain.Enums
{
    [Description("等级类型")]
    public enum LevelType : byte
    {
        [Description("公开")]
        Public = 0,
        [Description("私有")]
        Privite = 1,
        [Description("限时")]
        Temporal = 2,
    }
}
