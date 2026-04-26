using LocationSystem.Application.GrapqLDTOs.Menus;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Menus.Queries.GetMenus
{
    public class GetMenusQuery : IRequest<IQueryable<MenuGraphqLDto>>
    {
    }
}
