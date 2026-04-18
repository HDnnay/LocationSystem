using System.ComponentModel;

namespace LocationSystem.Domain.Enums
{
    public enum ArticleLevel : byte
    {
        [Description("公开")]
        Public = 0,
        [Description("私有")]
        Private = 1,
        [Description("限时可见")]
        Temporal = 3,
    }
}
