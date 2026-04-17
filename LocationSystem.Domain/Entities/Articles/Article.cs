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
        public virtual ICollection<ArticleTag>? Tags { get; private set; }
        [Description("评论")]
        public virtual ICollection<ArticleComment>? Comments { get; private set; }
        [Description("创建者")]

        public virtual User? CreateUser { get; set; }

        [Description("文章等级")]
        public ArticleLevel Level { get; set; } = ArticleLevel.Public;
        [Description("限时可见开始时间")]
        public DateTime? VisibleStartTime { get; private set; }
        [Description("限时可见结束时间")]
        public DateTime? VisibleEndTime { get; private set; }
        
        public bool IsCurrentlyVisible()
        {
            if (Level != ArticleLevel.Temporal)
            {
                return true;
            }
            
            var now = DateTime.Now;
            if (VisibleStartTime.HasValue && now < VisibleStartTime.Value)
            {
                return false;
            }
            if (VisibleEndTime.HasValue && now > VisibleEndTime.Value)
            {
                return false;
            }
            return true;
        }
        // 添加更新方法
        public void Update(string title, string content, bool isVisiable, string? topic, string? subtitle)
        {
            Title = title;
            Content = content;
            IsVisiable = isVisiable;
            Topic = topic;
            Subtitle = subtitle;
        }

        public void SetVisibleTimeRange(DateTime? startTime, DateTime? endTime)
        {
            VisibleStartTime = startTime;
            VisibleEndTime = endTime;
        }

        // 添加更新标签方法
        public void UpdateTags(List<ArticleTag> tags)
        {
            Tags = tags;
        }
    }

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
