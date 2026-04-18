namespace LocationSystem.Application.Utilities.Common
{
    public class PageResult<T>
    {
        public List<T>? Items { get; set; }
        public int Total { get; set; }
        public int CurrentPage { get; set; }
    }
}
