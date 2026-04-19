namespace LocationSystem.Application.Utilities.Common
{
    public class PageRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? KeyWord { get; set; } = string.Empty;
    }
}
