using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Articles.Queries.GetArticles
{
    public class GetArticlesQuery : IRequest<IQueryable<ArticleGraphqLDto>>
    {
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; }
    }
}
