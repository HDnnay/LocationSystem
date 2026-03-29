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
    public class ArticleDataLoader : BatchDataLoader<Guid, Article>
    {
        private readonly IServiceProvider _serviceProvider;

        public ArticleDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider)
            : base(batchScheduler, new DataLoaderOptions())
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<IReadOnlyDictionary<Guid, Article>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var articleRepository = scope.ServiceProvider.GetRequiredService<IArticleRepository>();

            var result = new Dictionary<Guid, Article>();

            foreach (var id in keys)
            {
                var article = await articleRepository.GetByIdAsync(id, true);
                if (article != null)
                {
                    result[id] = article;
                }
            }

            return result;
        }
    }
}
