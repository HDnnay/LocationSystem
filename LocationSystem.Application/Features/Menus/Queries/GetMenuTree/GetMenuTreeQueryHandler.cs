using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Menus;

namespace LocationSystem.Application.Features.Menus.Queries.GetMenuTree
{
    public class GetMenuTreeQueryHandler : IRequestHandler<GetMenuTreeQuery, List<MenuDto>>
    {
        private readonly IMenuRepository _menuRepository;

        public GetMenuTreeQueryHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<MenuDto>> Handle(GetMenuTreeQuery request)
        {
            var menus = await _menuRepository.GetMenuTreeAsync();
            return MapMenusToDto(menus);
        }

        private List<MenuDto> MapMenusToDto(IEnumerable<Menu> menus)
        {
            return menus?.Select(menu => new MenuDto
            {
                Id = menu.Id,
                Name = menu.Name,
                Path = menu.Path,
                Icon = menu.Icon,
                Order = menu.Order,
                Level = menu.Level,
                ParentId = menu.ParentId,
                CreatedAt = menu.CreateTime,
                UpdatedAt = menu.UpdatedAt,
                ChildMenus = MapMenusToDto(menu.Children)
            }).ToList() ?? new List<MenuDto>();
        }
    }
}