using LocationSystem.Domain.Enums;
using System.ComponentModel;

namespace LocationSystem.Application.GrapqLDTOs.Articles
{
    public class ArticleGraphqLDto
    {
        [Description("Id")]
        public Guid Id { get; set; }
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }
        [Description("内容")]

        public string Content { get; set; }
        [Description("标题")]

        public string Title { get; set; }
        [Description("副标题")]

        public string? Subtitle { get; set; }

        [Description("作者Id")]

        public Guid UserId { get; set; }
        [Description("删除人Id")]

        public Guid? DeleteUserId { get; set; }
        [Description("删除时间")]

        public DateTime DeleteTime { get; set; }
        [Description("是否禁用")]

        public bool IsDisabled { get; set; }
        [Description("主题")]


        public string? Topic { get; set; }
        [Description("Id")]

        public bool IsDelete { get; set; }
        [Description("文章可见等级")]

        public ArticleLevel Level { get; set; }
        [Description("文章可见开始时间")]

        public DateTime? VisibleStartTime { get; set; }
        [Description("文章可见结束时间")]

        public DateTime? VisibleEndTime { get; set; }
    }
}
