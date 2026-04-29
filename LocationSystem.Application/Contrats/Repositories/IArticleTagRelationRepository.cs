using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Application.Contrats.Repositories
{
    public interface IArticleTagRelationRepository : IRepository<ArticleTagRelation>
    {
        /// <summary>
        /// 根据文章ID删除所有标签关联
        /// </summary>
        /// <param name="articleId">文章ID</param>
        Task RemoveByArticleIdAsync(Guid articleId);
        
        /// <summary>
        /// 根据文章ID获取所有标签关联
        /// </summary>
        /// <param name="articleId">文章ID</param>
        Task<List<ArticleTagRelation>> GetByArticleIdAsync(Guid articleId);
        
        /// <summary>
        /// 批量添加标签关联
        /// </summary>
        /// <param name="relations">标签关联列表</param>
        Task AddRangeAsync(IEnumerable<ArticleTagRelation> relations);
    }
}