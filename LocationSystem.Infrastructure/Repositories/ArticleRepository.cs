using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Domain.Entities.Articles;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Infrastructure.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly AppDbContext _context;

        public ArticleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Article?> GetByIdAsync(Guid id, bool includeTags = false)
        {
            var query = _context.Articles.AsQueryable();

            if (includeTags)
            {
                query = query.Include(a => a.Tags);
            }

            return await query.FirstOrDefaultAsync(a => a.Id == id);
        }

        public IQueryable<Article> GetAllQueryable()
        {
            return _context.Articles.AsQueryable();
        }

        public async Task<List<Article>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Articles
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
        }

        public async Task<Dictionary<Guid, ICollection<ArticleTag>>> GetTagsByArticleIdsAsync(List<Guid> articleIds)
        {
            var articles = await _context.Articles
                .Include(a => a.Tags)
                .Where(a => articleIds.Contains(a.Id))
                .ToListAsync();

            var result = new Dictionary<Guid, ICollection<ArticleTag>>();

            foreach (var article in articles)
            {
                result[article.Id] = article.Tags ?? new List<ArticleTag>();
            }

            // 确保所有请求的文章都有结果
            foreach (var articleId in articleIds)
            {
                if (!result.ContainsKey(articleId))
                {
                    result[articleId] = new List<ArticleTag>();
                }
            }

            return result;
        }

        public async Task<Dictionary<Guid, ICollection<ArticleComment>>> GetCommentsByArticleIdsAsync(List<Guid> articleIds)
        {
            var articles = await _context.Articles
                .Include(a => a.Comments)
                .Where(a => articleIds.Contains(a.Id))
                .ToListAsync();

            var result = new Dictionary<Guid, ICollection<ArticleComment>>();

            foreach (var article in articles)
            {
                result[article.Id] = article.Comments ?? new List<ArticleComment>();
            }

            // 确保所有请求的文章都有结果
            foreach (var articleId in articleIds)
            {
                if (!result.ContainsKey(articleId))
                {
                    result[articleId] = new List<ArticleComment>();
                }
            }

            return result;
        }
    }


}