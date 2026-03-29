using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using System;

namespace LocationSystem.Application.Features.Articles.Queries.GetArticle
{
    public class GetArticleQuery : IRequest<ArticleDto>
    {
        public Guid Id { get; set; }
    }
}
