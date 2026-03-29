using LocationSystem.Domain.Entities.Interfacies;

namespace LocationSystem.Domain.Entities.Articles
{
    public class ArticleComment : IEntityVisiable
    {
        private ArticleComment() { Id = Guid.NewGuid(); CreateTiem = DateTime.Now; }
        public ArticleComment(Guid userId, string comment, bool isVisiable)
        {
            UserId = userId;
            Comment = comment;
            IsVisiable = isVisiable;
        }
        public Guid UserId { get; private set; }
        public string Comment { get; set; }
        public Guid Id { get; private set; }

        public bool IsVisiable { get; set; }
        public DateTime CreateTiem { get; set; }

        public void UpdateComment(string newComment)
        {
            Comment = newComment;
        }
        public void UpdateVisiable(bool isVisiable)
        {
            IsVisiable = isVisiable;
        }
    }
}
