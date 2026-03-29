using System;
using System.Collections.Generic;

namespace LocationSystem.Application.Features.Articles.Queries.GetArticles
{
    public class GetArticlesInput
    {
        public int? First { get; set; }
        public int? Skip { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; }
        public string? Filter { get; set; }
    }
}
