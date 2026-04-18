using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Enums;
using Mapster;

namespace LocationSystem.Application.Features.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, ArticleDto>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateArticleCommandHandler(IArticleRepository articleRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
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

            return article.Adapt<ArticleDto>();
        }
    }
}
