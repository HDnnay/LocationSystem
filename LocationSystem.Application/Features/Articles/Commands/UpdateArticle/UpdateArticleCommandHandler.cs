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

namespace LocationSystem.Application.Features.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, ArticleDto>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateArticleCommandHandler(IArticleRepository articleRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(UpdateArticleCommand command)
        {
            var article = await _articleRepository.GetByIdAsync(command.Id);
            if (article == null)
            {
                throw new Exception($"文章不存在，ID: {command.Id}");
            }

            // 使用 Article 实体的 Update 方法更新属性
            article.Update(
                command.Title,
                command.Content,
                command.IsVisiable,
                command.Topic,
                command.Subtitle
            );

            if (command.TagIds != null)
            {
                var tags = await _tagRepository.GetByIdsAsync(command.TagIds);
                article.UpdateTags(tags.ToList());
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _articleRepository.UpdateAsync(article);
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
