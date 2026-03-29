using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Utilities;
using System;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Articles.Commands.DeleteArticle
{
    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, SuccessResponse>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
        {
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SuccessResponse> Handle(DeleteArticleCommand command)
        {
            var article = await _articleRepository.GetByIdAsync(command.ArticleId);
            if (article == null)
            {
                throw new Exception($"文章不存在，ID: {command.ArticleId}");
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _articleRepository.DeleteAsync(article);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return new SuccessResponse { Success = true };
        }
    }
}
