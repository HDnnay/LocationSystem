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
    public class ArticleCommentsDataLoader : BatchDataLoader<Guid, ICollection<ArticleComment>>
    {
        private readonly IServiceProvider _serviceProvider;

        public ArticleCommentsDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider)
            : base(batchScheduler, new DataLoaderOptions())
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<IReadOnlyDictionary<Guid, ICollection<ArticleComment>>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var commentRepository = scope.ServiceProvider.GetRequiredService<ICommentRepository>();

            var comments = await commentRepository.GetByArticleIdsAsync(keys);
            var result = new Dictionary<Guid, ICollection<ArticleComment>>();

            foreach (var id in keys)
            {
                result[id] = comments.Where(c => c.ArticleId == id).ToList();
            }

            return result;
        }
    }
}
