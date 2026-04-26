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

        public async Task<Dictionary<Guid, List<ArticleTagGraphqLDto>>> GetTagsByArticleIdsAsync(IReadOnlyList<Guid> articleIds, CancellationToken cancellationToken = default)
        {
            if (articleIds == null || !articleIds.Any())
                return new Dictionary<Guid, List<ArticleTagGraphqLDto>>();

            // 查询所有指定文章的文章标签
            var articles = await _context.Articles
                .Include(a => a.Tags)
                .Where(a => articleIds.Contains(a.Id))
                .ToListAsync(cancellationToken);

            // 创建字典，按文章ID分组
            var result = new Dictionary<Guid, List<ArticleTagGraphqLDto>>();

            foreach (var article in articles)
            {
                var tagDtos = new List<ArticleTagGraphqLDto>();

                if (article.Tags != null)
                {
                    foreach (var tag in article.Tags)
                    {
                        var tagDto = tag.Adapt<ArticleTagGraphqLDto>();
                        tagDtos.Add(tagDto);
                    }
                }

                result[article.Id] = tagDtos;
            }

            // 确保所有请求的文章都有结果（即使没有标签）
            foreach (var articleId in articleIds)
            {
                if (!result.ContainsKey(articleId))
                {
                    result[articleId] = new List<ArticleTagGraphqLDto>();
                }
            }

            return result;
        }
    }
}
