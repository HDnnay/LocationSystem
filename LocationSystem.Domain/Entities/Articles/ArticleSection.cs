using LocationSystem.Domain.Entities.Interfacies;
using LocationSystem.Domain.Entities.UserRolePermissions;
using System.ComponentModel;

namespace LocationSystem.Domain.Entities.Articles
{
    [Description("文章板块")]
    public class ArticleSection : IEntityVisiable
    {
        public bool IsVisiable { get; set; }
        public Guid? UserId { get; set; }
        public Guid Id { get; private set; }

        public DateTime CreateTime { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public string ICon { get; set; }
        public virtual User? User { get; private set; }
        public virtual ICollection<Article>? Articles { get; set; }

    }
}
