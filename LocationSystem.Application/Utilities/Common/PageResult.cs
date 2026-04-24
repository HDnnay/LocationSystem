namespace LocationSystem.Application.Utilities.Common
{
    public class PageResult<T>
    {
        public List<T>? Items { get; set; }
        public int Total { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage => CurrentPage * PageSize < Total;
        public bool HasPreviousPage => CurrentPage > 1;
    }
}
