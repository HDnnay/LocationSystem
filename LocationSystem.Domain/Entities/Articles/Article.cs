using LocationSystem.Domain.Entities.Interfacies;
using LocationSystem.Domain.Entities.UserRolePermissions;
using System.ComponentModel;

namespace LocationSystem.Domain.Entities.Articles
{
    public class Article : IEntityVisiable
    {
        public Guid Id { get; private set; }
        public DateTime CreateTiem { get; private set; }
        [Description("内容")]
        public string Content { get; private set; }
        [Description("标题")]
        public string Title { get; private set; }
        [Description("副标题")]
        public string? Subtitle { get; private set; }
        [Description("是否可见")]

        public bool IsVisiable { get; set; }
        public Guid UserId { get; set; }
        [Description("主题")]
        public string? Topic { get; private set; }
        [Description("标签")]
        public virtual ICollection<Tag>? Tags { get; private set; }
        [Description("评论")]
        public virtual ICollection<ArticleComment>? Comments { get; private set; }

        public virtual User? User { get; set; }
    }
}
