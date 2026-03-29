using System;

namespace LocationSystem.Application.Utilities
{
    public class PaginationInput
    {
        public int? First { get; set; }
        public int? Skip { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; }
        public string? Filter { get; set; }
    }
}
