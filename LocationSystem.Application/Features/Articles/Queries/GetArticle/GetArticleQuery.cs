using LocationSystem.Application.Dtos.Articles;
using LocationSystem.Application.Utilities;
using System;

namespace LocationSystem.Application.Features.Articles.Queries.GetArticle
{
    public class GetArticleQuery : IRequest<ArticleDto>
    {
        public Guid Id { get; set; }
    }
}
