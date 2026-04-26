using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Articles.Queries.GetCommentsByArticleIds
{
    public class GetCommentsByArticleIdsQueryHandler : IRequestHandler<GetCommentsByArticleIdsQuery, List<ArticleCommentGraphqLDto>>
    {
        private readonly IArticleCommentRepository _repository;

        public GetCommentsByArticleIdsQueryHandler(IArticleCommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ArticleCommentGraphqLDto>> Handle(GetCommentsByArticleIdsQuery request)
        {
            return await _repository.GetCommentsByArticleIdsAsync(request.ArticleIds);
        }
    }
}