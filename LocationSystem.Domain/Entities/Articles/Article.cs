using LocationSystem.Domain.Entities.Interfacies;
using System.ComponentModel;

namespace LocationSystem.Domain.Entities.Articles
{
    public class Article : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateTiem { get; set; }
        [Description("内容")]
        public required string Content { get; set; }
        [Description("标题")]

        public required string Title { get; set; }
        [Description("副标题")]
        public string? Subtitle { get; set; }
        [Description("主题")]

        public string Topic { get; set; }
        [Description("标签")]
        public string Tag { get; set; }


    }
}
