using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleImages.Queries.GetArticleImages
{
    public class GetArticleImageQueryHandler : IRequestHandler<GetArticleImageQuery, IQueryable<ArticleImageGraphqLDto>>
    {
        public Task<IQueryable<ArticleImageGraphqLDto>> Handle(GetArticleImageQuery request)
        {
            throw new NotImplementedException();
        }
    }
}
