using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleTags.Queries.GetArticleTags
{
    public class GetArticleTagsQueryHandler : IRequestHandler<GetArticleTagsQuery, IQueryable<ArticleTagGraphqLDto>>
    {
        Task<IQueryable<ArticleTagGraphqLDto>> IRequestHandler<GetArticleTagsQuery, IQueryable<ArticleTagGraphqLDto>>.Handle(GetArticleTagsQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
