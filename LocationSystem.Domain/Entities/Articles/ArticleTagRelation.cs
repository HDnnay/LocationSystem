using LocationSystem.Domain.Entities.Interfacies;

namespace LocationSystem.Domain.Entities.Articles
{
    public class ArticleTagRelation : IEntity
    {
        public Guid ArticleId { get; set; }
        public Guid TagId { get; set; }
        
        // 导航属性
        public virtual Article Article { get; set; }
        public virtual ArticleTag Tag { get; set; }
        
        // IEntity 接口实现
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
    }
}