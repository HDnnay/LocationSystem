using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleComments.Queries.GetArticleCommentByIds
{
    public class GetCommentByArticleIdsQuery : IRequest<Dictionary<Guid, ArticleCommentGraphqLDto>>
    {
        public IReadOnlyList<Guid> Ids { get; set; }
    }
}
