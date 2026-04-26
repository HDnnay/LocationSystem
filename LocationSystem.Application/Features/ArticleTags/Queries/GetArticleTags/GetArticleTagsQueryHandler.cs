using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.ArticleTags.Queries.GetArticleTags
{
    public class GetArticleTagsQueryHandler(IArticleTagRepository repository) : IRequestHandler<GetArticleTagsQuery, IQueryable<ArticleTagGraphqLDto>>
    {
        public async Task<IQueryable<ArticleTagGraphqLDto>> Handle(GetArticleTagsQuery request)
        {
            var result = repository.Query().ProjectToType<ArticleTagGraphqLDto>();
            return await Task.FromResult(result);
        }
    }
}
