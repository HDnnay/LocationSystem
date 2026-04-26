using System.ComponentModel;

namespace LocationSystem.Application.GrapqLDTOs.Articles
{
    public class ArticleTagGraphqLDto
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Description("是否可见")]
        public bool IsVisiable { get; set; }
    }
}
