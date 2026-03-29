using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Articles;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Articles.Commands.CreateArticle
{
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, ArticleDto>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateArticleCommandHandler(IArticleRepository articleRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(CreateArticleCommand command)
        {
            var article = new Article(
                command.Title,
                command.Content,
                command.IsVisiable,
                command.UserId,
                command.Topic,
                command.Subtitle
            );

            if (command.TagIds != null && command.TagIds.Any())
            {
                var tags = await _tagRepository.GetByIdsAsync(command.TagIds);
                article.UpdateTags(tags.ToList());
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _articleRepository.AddAsync(article);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return _mapper.Map<ArticleDto>(article);
        }
    }
}
