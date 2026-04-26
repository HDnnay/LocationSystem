using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class TagRepository : Repository<ArticleTag>, IArticleTagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleTag>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.ArticleTags
                .Where(t => ids.Contains(t.Id))
                .ToListAsync();
        }

        public async Task<ILookup<Guid, ArticleTagGraphqLDto>> GetTagsByArticleIdsAsync(IReadOnlyList<Guid> articleIds, CancellationToken cancellationToken = default)
        {
            if (articleIds == null || !articleIds.Any())
                return Enumerable.Empty<(Guid ArticleId, ArticleTagGraphqLDto Tag)>().ToLookup(x => x.ArticleId, x => x.Tag);

            // 查询所有指定文章的文章标签
            var articles = await _context.Articles
                .Include(a => a.Tags)
                .Where(a => articleIds.Contains(a.Id))
                .ToListAsync(cancellationToken);

            // 创建包含文章ID和标签信息的列表
            var tagPairs = new List<(Guid ArticleId, ArticleTagGraphqLDto Tag)>();

            foreach (var article in articles)
            {
                if (article.Tags != null)
                {
                    foreach (var tag in article.Tags)
                    {
                        var tagDto = tag.Adapt<ArticleTagGraphqLDto>();
                        tagPairs.Add((article.Id, tagDto));
                    }
                }
            }

            // 直接返回 ILookup
            return tagPairs.ToLookup(item => item.ArticleId, item => item.Tag);
        }
    }
}
