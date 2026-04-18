using LocationSystem.Domain.Enums;

namespace LocationSystem.Application.Dtos
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public DateTime CreateTiem { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string? Subtitle { get; set; }
        public bool IsVisiable { get; set; }
        public Guid UserId { get; set; }
        public string? Topic { get; set; }
        public List<TagDto>? Tags { get; set; }
        public List<ArticleCommentDto>? Comments { get; set; }
        public UserDto? CreateUser { get; set; }
        public ArticleLevel Level { get; set; }
        public DateTime? VisibleStartTime { get; set; }
        public DateTime? VisibleEndTime { get; set; }
    }

    public class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class ArticleCommentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Comment { get; set; }
        public bool IsVisiable { get; set; }
        public DateTime CreateTiem { get; set; }
    }
}