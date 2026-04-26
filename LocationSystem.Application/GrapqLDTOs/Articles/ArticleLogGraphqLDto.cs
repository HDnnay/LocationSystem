using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Application.GrapqLDTOs.Articles
{
    public class ArticleLogGraphqLDto
    {
        public Guid Id { get; private set; }

        public DateTime CreateTime { get; private set; }

        public Guid? ArticleId { get; set; }
        public virtual Article? Article { get; private set; }
        public ArticleState State { get; set; }
        public string Log { get; private set; }
        public Guid UserId { get; set; }
    }
}
