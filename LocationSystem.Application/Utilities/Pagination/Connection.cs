namespace LocationSystem.Application.Utilities.Pagination
{
    public class Connection<T>
    {
        public List<Edge<T>> Edges { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public class Edge<T>
    {
        public string Cursor { get; set; }
        public T Node { get; set; }
    }

    public class PageInfo
    {
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public string StartCursor { get; set; }
        public string EndCursor { get; set; }
    }
}
