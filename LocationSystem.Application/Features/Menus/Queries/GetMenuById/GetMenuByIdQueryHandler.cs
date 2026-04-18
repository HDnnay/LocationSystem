using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Menus;
using LocationSystem.Application.Utilities;
using Mapster;

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

            return menu.Adapt<MenuDto>();
        }
    }
}
