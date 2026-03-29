using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Articles;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Articles.Queries.GetArticles
{
    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, IQueryable<Article>>
    {
        private readonly IArticleRepository _articleRepository;

        public GetArticlesQueryHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IQueryable<Article>> Handle(GetArticlesQuery request)
        {
            var query = _articleRepository.GetAllQueryable();

            // 处理排序
            if (!string.IsNullOrEmpty(request.Pagination?.SortBy))
            {
                var sortBy = request.Pagination.SortBy;
                var sortDescending = request.Pagination.SortDescending ?? false;

                // 根据排序字段进行排序
                switch (sortBy.ToLower())
                {
                    case "id":
                        query = sortDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
                        break;
                    case "title":
                        query = sortDescending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title);
                        break;
                    case "topic":
                        query = sortDescending ? query.OrderByDescending(x => x.Topic) : query.OrderBy(x => x.Topic);
                        break;
                    case "createtime":
                    case "createtiem":
                        query = sortDescending ? query.OrderByDescending(x => x.CreateTiem) : query.OrderBy(x => x.CreateTiem);
                        break;
                    default:
                        // 默认按创建时间降序排序
                        query = query.OrderByDescending(x => x.CreateTiem);
                        break;
                }
            }
            else
            {
                // 默认按创建时间降序排序
                query = query.OrderByDescending(x => x.CreateTiem);
            }

            return query;
        }
    }
}
