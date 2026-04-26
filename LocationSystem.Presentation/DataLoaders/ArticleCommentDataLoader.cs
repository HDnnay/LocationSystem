using LocationSystem.Application.Features.Articles.Queries.GetCommentsByArticleIds;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation.DataLoaders
{
    public class ArticleCommentDataLoader : GroupedDataLoader<Guid, ArticleCommentGraphqLDto>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ArticleCommentDataLoader(
            IServiceScopeFactory scopeFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task<ILookup<Guid, ArticleCommentGraphqLDto>> LoadGroupedBatchAsync(
            IReadOnlyList<Guid> articleIds,
            CancellationToken cancellationToken)
        {
            // 创建临时作用域，确保 DbContext 隔离
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var query = new GetCommentsByArticleIdsQuery { ArticleIds = articleIds };
            var comments = await mediator.Send(query);

            // 返回 ILookup<Guid, ArticleCommentGraphqLDto>
            // 每个文章ID对应一个评论列表
            return comments.ToLookup(comment => comment.ArticleId);
        }
    }
}