using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleComments.Queries.GetArticleCommentByIds
{
    public class GetCommentByArticleIdsHandler(IArticleCommentRepository repository) : IRequestHandler<GetCommentByArticleIdsQuery, Dictionary<Guid, ArticleCommentGraphqLDto>>
    {
        public Task<Dictionary<Guid, ArticleCommentGraphqLDto>> Handle(GetCommentByArticleIdsQuery request)
        {
            var reuslt = repository.GetByArticleIdsAsync(request.Ids);
        }
    }
}
