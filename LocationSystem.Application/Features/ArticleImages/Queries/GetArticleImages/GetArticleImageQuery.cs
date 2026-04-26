using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.ArticleImages.Queries.GetArticleImages
{
    public class GetArticleImageQuery : IRequest<IQueryable<ArticleImageGraphqLDto>>
    {
    }
}
