using LocationSystem.Application.Features.Articles.Queries.GetTagsByArticleIds;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation.DataLoaders
{
    public class ArticleTagDataLoader : GroupedDataLoader<Guid, ArticleTagGraphqLDto>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ArticleTagDataLoader(
            IServiceScopeFactory scopeFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task<ILookup<Guid, ArticleTagGraphqLDto>> LoadGroupedBatchAsync(
            IReadOnlyList<Guid> articleIds,
            CancellationToken cancellationToken)
        {
            // 创建临时作用域，确保 DbContext 隔离
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var query = new GetTagsByArticleIdsQuery { ArticleIds = articleIds };
            var tagsLookup = await mediator.Send(query);

            // 直接返回 ILookup，无需转换
            return tagsLookup;
        }
    }
}