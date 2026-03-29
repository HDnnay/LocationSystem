using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Articles;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Articles.Queries.GetArticle
{
    public class GetArticleQueryHandler : IRequestHandler<GetArticleQuery, ArticleDto>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public GetArticleQueryHandler(IArticleRepository articleRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(GetArticleQuery request)
        {
            var article = await _articleRepository.GetByIdAsync(request.Id, true);
            if (article == null)
            {
                throw new Exception($"文章不存在，ID: {request.Id}");
            }
            return _mapper.Map<ArticleDto>(article);
        }
    }
}
