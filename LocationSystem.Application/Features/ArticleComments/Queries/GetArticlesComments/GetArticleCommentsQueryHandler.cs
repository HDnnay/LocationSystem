using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleComments.Queries.GetArticlesComments
{
    public class GetArticleCommentsQueryHandler : IRequestHandler<GetArticleCommentsQuery, IQueryable<ArticleCommentGraphqLDto>>
    {
        public Task<IQueryable<ArticleCommentGraphqLDto>> Handle(GetArticleCommentsQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
