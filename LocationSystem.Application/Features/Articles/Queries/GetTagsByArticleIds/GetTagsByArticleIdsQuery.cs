using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Articles.Queries.GetTagsByArticleIds
{
    public class GetTagsByArticleIdsQuery : IRequest<ILookup<Guid, ArticleTagGraphqLDto>>
    {
        public IReadOnlyList<Guid> ArticleIds { get; set; }
    }
}