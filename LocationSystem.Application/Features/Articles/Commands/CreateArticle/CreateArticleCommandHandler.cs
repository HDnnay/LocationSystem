using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos.Articles;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Articles;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Articles.Commands.CreateArticle
{
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, ArticleDto>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleTagRepository _tagRepository;
        private readonly IArticleTagRelationRepository _articleTagRelationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateArticleCommandHandler(
            IArticleRepository articleRepository, 
            IArticleTagRepository tagRepository,
            IArticleTagRelationRepository articleTagRelationRepository,
            IUnitOfWork unitOfWork)
        {
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
            _articleTagRelationRepository = articleTagRelationRepository;
            _unitOfWork = unitOfWork;
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
            
            article.Level = command.Level;
            article.SetVisibleTimeRange(command.VisibleStartTime, command.VisibleEndTime);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _articleRepository.AddAsync(article);
                
                // 处理标签关联
                if (command.TagIds != null && command.TagIds.Any())
                {
                    var tags = await _tagRepository.GetByIdsAsync(command.TagIds);
                    var relations = tags.Select(tag => new ArticleTagRelation
                    {
                        ArticleId = article.Id,
                        TagId = tag.Id
                    }).ToList();
                    
                    await _articleTagRelationRepository.AddRangeAsync(relations);
                }
                
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return article.Adapt<ArticleDto>();
        }
    }
}
