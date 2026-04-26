using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleLogs.Queries.GetAricleLogs
{
    public class GetArticleQueryHandler : IRequestHandler<GetArticleLogsQuery, IQueryable<ArticleLogGraphqLDto>>
    {
        public Task<IQueryable<ArticleLogGraphqLDto>> Handle(GetArticleLogsQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
