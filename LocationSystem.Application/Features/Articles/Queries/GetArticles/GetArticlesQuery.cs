using LocationSystem.Domain.Entities.Articles;
using LocationSystem.Application.Utilities;
using System;

namespace LocationSystem.Application.Features.Articles.Queries.GetArticles
{
    public class GetArticlesQuery : IRequest<IQueryable<Article>>
    {
        public PaginationInput? Pagination { get; set; }
    }
}
