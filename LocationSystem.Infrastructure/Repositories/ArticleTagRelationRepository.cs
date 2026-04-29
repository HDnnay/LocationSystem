using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.Articles;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleTagRelationRepository : Repository<ArticleTagRelation>, IArticleTagRelationRepository
    {
        public ArticleTagRelationRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 根据文章ID删除所有标签关联
        /// </summary>
        /// <param name="articleId">文章ID</param>
        public async Task RemoveByArticleIdAsync(Guid articleId)
        {
            var relations = await _context.ArticleTagRelations
                .Where(r => r.ArticleId == articleId)
                .ToListAsync();
                
            if (relations.Any())
            {
                _context.ArticleTagRelations.RemoveRange(relations);
            }
        }

        /// <summary>
        /// 根据文章ID获取所有标签关联
        /// </summary>
        /// <param name="articleId">文章ID</param>
        public async Task<List<ArticleTagRelation>> GetByArticleIdAsync(Guid articleId)
        {
            return await _context.ArticleTagRelations
                .Where(r => r.ArticleId == articleId)
                .Include(r => r.Tag) // 包含标签信息
                .ToListAsync();
        }

        /// <summary>
        /// 批量添加标签关联
        /// </summary>
        /// <param name="relations">标签关联列表</param>
        public async Task AddRangeAsync(IEnumerable<ArticleTagRelation> relations)
        {
            if (relations != null && relations.Any())
            {
                await _context.ArticleTagRelations.AddRangeAsync(relations);
            }
        }

        /// <summary>
        /// 根据文章ID和标签ID获取关联
        /// </summary>
        public async Task<ArticleTagRelation?> GetByArticleAndTagAsync(Guid articleId, Guid tagId)
        {
            return await _context.ArticleTagRelations
                .FirstOrDefaultAsync(r => r.ArticleId == articleId && r.TagId == tagId);
        }

        /// <summary>
        /// 根据标签ID删除所有关联
        /// </summary>
        public async Task RemoveByTagIdAsync(Guid tagId)
        {
            var relations = await _context.ArticleTagRelations
                .Where(r => r.TagId == tagId)
                .ToListAsync();
                
            if (relations.Any())
            {
                _context.ArticleTagRelations.RemoveRange(relations);
            }
        }
    }
}