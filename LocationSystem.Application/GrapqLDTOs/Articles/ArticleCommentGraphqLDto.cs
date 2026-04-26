namespace LocationSystem.Application.GrapqLDTOs.Articles
{
    public class ArticleCommentGraphqLDto
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Guid UserId { get; set; }
        public string Comment { get; set; }
        public bool IsVisiable { get; set; }
        public DateTime CreateTime { get; set; }
    }
}