using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Articles.Queries.GetTagsByArticleIds
{
    public class GetTagsByArticleIdsQueryHandler : IRequestHandler<GetTagsByArticleIdsQuery, Dictionary<Guid, List<ArticleTagGraphqLDto>>>
    {
        private readonly IArticleTagRepository _repository;

        public GetTagsByArticleIdsQueryHandler(IArticleTagRepository repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<Guid, List<ArticleTagGraphqLDto>>> Handle(GetTagsByArticleIdsQuery request)
        {
            return await _repository.GetTagsByArticleIdsAsync(request.ArticleIds);
        }
    }
}