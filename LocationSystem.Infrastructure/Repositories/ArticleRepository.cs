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
    }


}