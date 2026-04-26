using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Articles.Queries.GetCommentsByArticleIds
{
    public class GetCommentsByArticleIdsQuery : IRequest<List<ArticleCommentGraphqLDto>>
    {
        public IReadOnlyList<Guid> ArticleIds { get; set; }
    }
}