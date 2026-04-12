using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Articles;
using Mapster;
using System;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Articles.Queries.GetArticle
{
    public class GetArticleQueryHandler : IRequestHandler<GetArticleQuery, ArticleDto>
    {
        private readonly IArticleRepository _articleRepository;
        public GetArticleQueryHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<ArticleDto> Handle(GetArticleQuery query)
        {
            var article = await _articleRepository.GetByIdAsync(query.Id);
            if (article == null)
            {
                throw new Exception("文章不存在");
            }

            return article.Adapt<ArticleDto>();
        }
    }
}
