using LocationSystem.Domain.Entities.Interfacies;
using LocationSystem.Domain.Entities.UserRolePermissions;
using System.ComponentModel;

namespace LocationSystem.Domain.Entities.Articles
{
    [Description("文章审核记录")]
    public class ArticleLog : IEntity
    {
        public Guid Id { get; private set; }

        public DateTime CreateTime { get; private set; }

        public Guid? ArticleId { get; set; }
        public virtual Article? Article { get; private set; }
        public ArticleState State { get; set; }
        public string Log { get; private set; }
        public Guid UserId { get; set; }
        public User? User { get; private set; }
    }
}
