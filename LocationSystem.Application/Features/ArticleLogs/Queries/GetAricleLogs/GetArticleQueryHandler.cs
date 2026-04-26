using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleLogs.Queries.GetAricleLogs
{
    public class GetArticleQueryHandler(IArticleLogRepository repository) : IRequestHandler<GetArticleLogsQuery, IQueryable<ArticleLogGraphqLDto>>
    {
        public Task<IQueryable<ArticleLogGraphqLDto>> Handle(GetArticleLogsQuery request)
        {
            repository
        }
    }
}
