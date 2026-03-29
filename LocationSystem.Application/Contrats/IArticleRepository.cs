using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Application.Contrats
{
    public interface IArticleRepository
    {
        Task<Article> GetByIdAsync(Guid id, bool includeRelated = false);
        Task<List<Article>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<List<Article>> GetAll();
        IQueryable<Article> GetAllQueryable();
        Task AddAsync(Article article);
        Task UpdateAsync(Article article);
        Task DeleteAsync(Article article);
        Task<int> SaveChangesAsync();
    }
}

public interface ITagRepository
{
    Task<Tag> GetByIdAsync(Guid id);
    Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<List<Tag>> GetAll();
}
