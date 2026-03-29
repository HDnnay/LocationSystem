using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Application.Features.Articles.Queries.GetArticles
{
    public class GetArticlesQuery : IRequest<IQueryable<Article>>
    {
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; }
    }
}
