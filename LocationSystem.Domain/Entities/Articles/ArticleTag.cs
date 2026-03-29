using LocationSystem.Domain.Entities.Interfacies;
using System.ComponentModel;

namespace LocationSystem.Domain.Entities.Articles
{
    public class ArticleTag : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateTiem { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Description("是否可见")]
        public bool IsVisiable { get; set; }

    }
}
