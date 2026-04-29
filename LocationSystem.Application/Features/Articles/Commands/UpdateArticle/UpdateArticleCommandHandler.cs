using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos.Articles;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Articles;
using LocationSystem.Domain.Enums;
using Mapster;

namespace LocationSystem.Application.Features.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, ArticleDto>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleTagRepository _tagRepository;
        private readonly IArticleTagRelationRepository _articleTagRelationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateArticleCommandHandler(
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

        public async Task<ArticleDto> Handle(UpdateArticleCommand command)
        {
            var article = await _articleRepository.GetByIdAsync(command.Id);
            if (article == null)
            {
                throw new Exception($"文章不存在，ID: {command.Id}");
            }

            article.Update(
                command.Title,
                command.Content,
                command.IsVisiable,
                command.Topic,
                command.Subtitle
            );

            article.Level = command.Level;

            if (command.Level == ArticleLevel.Temporal)
            {
                article.SetVisibleTimeRange(command.VisibleStartTime, command.VisibleEndTime);
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _articleRepository.UpdateAsync(article);

                // 处理标签关联
                if (command.TagIds != null)
                {
                    // 删除现有关联
                    await _articleTagRelationRepository.RemoveByArticleIdAsync(article.Id);

                    // 添加新关联
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
