using LocationSystem.Domain.Entities.Interfacies;
using System.ComponentModel;

namespace LocationSystem.Domain.Entities.Articles
{
    public class ArticleTag : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Description("是否可见")]
        public bool IsVisiable { get; set; }

        // 导航属性
        [Description("文章关联关系")]
        public virtual ICollection<ArticleTagRelation>? ArticleRelations { get; set; }


        public virtual ICollection<Article>? Articles { get; set; }
    }
}
