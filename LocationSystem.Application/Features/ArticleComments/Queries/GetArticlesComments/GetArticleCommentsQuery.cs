using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleComments.Queries.GetArticlesComments
{
    public class GetArticleCommentsQuery : IRequest<IQueryable<ArticleCommentGraphqLDto>>
    {
    }
}
