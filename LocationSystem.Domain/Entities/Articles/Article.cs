using LocationSystem.Domain.Entities.Interfacies;
using LocationSystem.Domain.Entities.UserRolePermissions;
using System.ComponentModel;

namespace LocationSystem.Domain.Entities.Articles
{
    public class Article : IEntityVisiable
    {
        private Article()
        {
            Id = Guid.NewGuid();
            CreateTiem = DateTime.Now;
        }
        public Article(string title, string content, bool isVisiable, Guid userId, string topic, string subtitle = default)
        {
            Title = title;
            Content = content;
            Topic = topic;
            Subtitle = subtitle;
            IsVisiable = isVisiable;
            UserId = userId;
        }
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
        [Description("创建者")]

        public virtual User? CreateUser { get; set; }
        public
    }
}
