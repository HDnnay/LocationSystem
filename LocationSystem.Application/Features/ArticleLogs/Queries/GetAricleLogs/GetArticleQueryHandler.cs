using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.ArticleLogs.Queries.GetAricleLogs
{
    public class GetArticleQueryHandler(IArticleLogRepository repository) : IRequestHandler<GetArticleLogsQuery, IQueryable<ArticleLogGraphqLDto>>
    {
        public async Task<IQueryable<ArticleLogGraphqLDto>> Handle(GetArticleLogsQuery request)
        {
            var model = repository.Query().ProjectToType<ArticleLogGraphqLDto>();
            return await Task.FromResult(model);
        }
    }
}
