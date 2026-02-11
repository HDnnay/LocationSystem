using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Menus.Queries.GetMenuById
{
    public class GetMenuByIdQueryHandler : IRequestHandler<GetMenuByIdQuery, MenuDto?>
    {
        private readonly IMenuRepository _menuRepository;

        public GetMenuByIdQueryHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<MenuDto?> Handle(GetMenuByIdQuery query)
        {
            var menu = await _menuRepository.GetByIdAsync(query.MenuId);
            if (menu == null)
            {
                return null;
            }

            return new MenuDto
            {
                Id = menu.Id,
                Name = menu.Name,
                Path = menu.Path,
                Icon = menu.Icon,
                Order = menu.Order,
                ParentId = menu.ParentId
            };
        }
    }
}
