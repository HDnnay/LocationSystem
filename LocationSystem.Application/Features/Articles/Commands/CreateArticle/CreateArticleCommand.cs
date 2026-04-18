using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Enums;

namespace LocationSystem.Application.Features.Articles.Commands.CreateArticle
{
    public class CreateArticleCommand : IRequest<ArticleDto>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsVisiable { get; set; }
        public Guid UserId { get; set; }
        public string? Topic { get; set; }
        public string? Subtitle { get; set; }
        public List<Guid>? TagIds { get; set; }
        public ArticleLevel Level { get; set; } = ArticleLevel.Public;
        public DateTime? VisibleStartTime { get; set; }
        public DateTime? VisibleEndTime { get; set; }
    }
}
