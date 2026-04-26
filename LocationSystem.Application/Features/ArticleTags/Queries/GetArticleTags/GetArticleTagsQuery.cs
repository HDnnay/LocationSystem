using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleTags.Queries.GetArticleTags
{
    public class GetArticleTagsQuery : IRequest<IQueryable<ArticleTagGraphqLDto>>
    {
    }
}
