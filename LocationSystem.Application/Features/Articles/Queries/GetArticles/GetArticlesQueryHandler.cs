using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.Articles.Queries.GetArticles
{
    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, IQueryable<ArticleGraphqLDto>>
    {
        private readonly IArticleRepository _articleRepository;

        public GetArticlesQueryHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IQueryable<ArticleGraphqLDto>> Handle(GetArticlesQuery request)
        {
            var query = _articleRepository.Query().ProjectToType<ArticleGraphqLDto>();

            return query;
        }
    }
}
