using LocationSystem.Application.Dtos.Menus;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Menus.Queries.GetMenuById
{
    public class GetMenuByIdQuery : IRequest<MenuDto?>
    {
        public Guid MenuId { get; set; }
    }
}
