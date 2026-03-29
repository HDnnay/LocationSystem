using LocationSystem.Application.Utilities;
using System;

namespace LocationSystem.Application.Features.Articles.Commands.DeleteArticle
{
    public class DeleteArticleCommand : IRequest<SuccessResponse>
    {
        public Guid ArticleId { get; set; }
    }
}
