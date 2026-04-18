using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Menus;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using Mapster;

namespace LocationSystem.Application.Features.Menus.Queries.GetAllMenus
{
    public class GetAllMenusQueryHandler : IRequestHandler<GetAllMenusQuery, PageResult<MenuDto>>
    {
        private readonly IMenuRepository _menuRepository;

        public GetAllMenusQueryHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<PageResult<MenuDto>> Handle(GetAllMenusQuery query)
        {
            var menus = await _menuRepository.GetMenuPage(query);
            var total = await _menuRepository.GetTotalCount();
            var pageResult = new PageResult<MenuDto>
            {
                Items = menus.Select(menu => menu.Adapt<MenuDto>()).ToList(),
                Total = total,
                CurrentPage = query.Page
            };
            return pageResult;
        }
    }
}
