using System.ComponentModel;

namespace LocationSystem.Domain.Entities.Articles
{
    public enum ArticleState
    {
        [Description("草稿")]
        Draft,
        [Description("审核中")]
        Auditing,
        [Description("拒绝")]

        Reject,
        [Description("发布")]

        Publish

    }
}
