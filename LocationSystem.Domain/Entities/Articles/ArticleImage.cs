using LocationSystem.Domain.Entities.Interfacies;

namespace LocationSystem.Domain.Entities.Articles
{
    public class ArticleImage : ISoftDeleteEntity
    {
        public bool IsDelete { get; set; }
        public Guid? DeleteUserId { get; set; }
        public DateTime DeleteTime { get; set; }
        public bool IsDisabled { get; set; }

        public Guid Id { get; private set; }
        public Guid? ArticelId { get; set; }
        public string? ImagePaths { get; set; }
        public DateTime CreateTime { get; private set; }
    }
}
