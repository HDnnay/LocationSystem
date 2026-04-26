using LocationSystem.Application.Features.Articles.Queries.GetCommentsByArticleIds;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.DataLoaders
{
    public class ArticleCommentDataLoader : GroupedDataLoader<Guid, ArticleCommentGraphqLDto>
    {
        private readonly IMediator _mediator;

        public ArticleCommentDataLoader(
            IMediator mediator,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _mediator = mediator;
        }

        protected override async Task<ILookup<Guid, ArticleCommentGraphqLDto>> LoadGroupedBatchAsync(
            IReadOnlyList<Guid> articleIds,
            CancellationToken cancellationToken)
        {
            var query = new GetCommentsByArticleIdsQuery { ArticleIds = articleIds };
            var comments = await _mediator.Send(query);

            // 返回 ILookup<Guid, ArticleCommentGraphqLDto>
            // 每个文章ID对应一个评论列表
            return comments.ToLookup(comment => comment.ArticleId);
        }
    }
}