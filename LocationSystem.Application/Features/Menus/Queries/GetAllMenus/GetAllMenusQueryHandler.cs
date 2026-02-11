using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;

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
                Data = menus.Select(menu => new MenuDto
                {
                    Id = menu.Id,
                    Name = menu.Name,
                    Path = menu.Path,
                    Icon = menu.Icon,
                    Order = menu.Order,
                    ParentId = menu.ParentId
                }).ToList(),
                Total = total,
                CurrentPage = query.Page
            };
            return pageResult;
        }
    }
}
