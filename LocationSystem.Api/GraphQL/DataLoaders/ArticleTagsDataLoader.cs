using GreenDonut;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.Articles;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocationSystem.Api.GraphQL.DataLoaders
{
    public class ArticleTagsDataLoader : BatchDataLoader<Guid, ICollection<LocationSystem.Domain.Entities.Articles.ArticleTag>>
    {
        private readonly IServiceProvider _serviceProvider;

        public ArticleTagsDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider)
            : base(batchScheduler, new DataLoaderOptions())
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<IReadOnlyDictionary<Guid, ICollection<LocationSystem.Domain.Entities.Articles.ArticleTag>>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var articleRepository = scope.ServiceProvider.GetRequiredService<IArticleRepository>();

            // 批量获取文章的标签
            var tagsByArticleId = await articleRepository.GetTagsByArticleIdsAsync(keys.ToList());

            return tagsByArticleId;
        }
    }
}
