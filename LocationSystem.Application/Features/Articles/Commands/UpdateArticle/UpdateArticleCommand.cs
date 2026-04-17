using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Articles;
using System;
using System.Collections.Generic;

namespace LocationSystem.Application.Features.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommand : IRequest<ArticleDto>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsVisiable { get; set; }
        public string? Topic { get; set; }
        public string? Subtitle { get; set; }
        public List<Guid>? TagIds { get; set; }
        public ArticleLevel Level { get; set; }
        public DateTime? VisibleStartTime { get; set; }
        public DateTime? VisibleEndTime { get; set; }
    }
}
