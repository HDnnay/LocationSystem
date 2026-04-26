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

            // 查询所有指定文章的标签关联关系
            var relations = await _context.ArticleTagRelations
                .Include(tr => tr.Tag)  // 包含标签信息
                .Where(tr => articleIds.Contains(tr.ArticleId))
                .ToListAsync(cancellationToken);

            // 创建包含文章ID和标签信息的列表
            var tagPairs = new List<(Guid ArticleId, ArticleTagGraphqLDto Tag)>();
            
            foreach (var relation in relations)
            {
                var tagDto = new ArticleTagGraphqLDto
                {
                    Id = relation.Tag.Id,
                    Name = relation.Tag.Name,
                    Description = relation.Tag.Description,
                    IsVisiable = relation.Tag.IsVisiable,
                    CreateTime = relation.Tag.CreateTime
                };
                tagPairs.Add((relation.ArticleId, tagDto));
            }

            // 直接返回 ILookup
            return tagPairs.ToLookup(item => item.ArticleId, item => item.Tag);
        }
    }
}
