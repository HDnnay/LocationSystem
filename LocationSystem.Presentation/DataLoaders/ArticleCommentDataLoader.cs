using LocationSystem.Application.Features.ArticleComments.Queries.GetArticleCommentByIds;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.DataLoaders
{
    public class ArticleCommentDataLoader : BatchDataLoader<Guid, ArticleCommentGraphqLDto>
    {
        private IMediator _mediator;
        public ArticleCommentDataLoader(IMediator mediator, IBatchScheduler batchScheduler, DataLoaderOptions options) : base(batchScheduler, options)
        {
            _mediator = mediator;
        }

        protected override async Task<IReadOnlyDictionary<Guid, ArticleCommentGraphqLDto>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            var query = new GetCommentByArticleIdsQuery() { Ids = keys };
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
