using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleLogs.Queries.GetAricleLogs
{
    public class GetArticleLogsQuery : IRequest<IQueryable<ArticleLogGraphqLDto>>
    {
    }
}
