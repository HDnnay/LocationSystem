using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;

namespace LocationSystem.Application.Features.Menus.Queries.GetAllMenus
{
    public class GetAllMenusQuery : IRequest<PageResult<MenuDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? keyWord { get; set; } = string.Empty;
    }
}
