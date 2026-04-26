using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.GrapqLDTOs.Menus;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.Menus.Queries.GetMenus
{
    public class GetMenusQueryHandler(IMenuRepository repository) : IRequestHandler<GetMenusQuery, IQueryable<MenuGraphqLDto>>
    {
        public Task<IQueryable<MenuGraphqLDto>> Handle(GetMenusQuery request)
        {
            var result = repository.Query().ProjectToType<MenuGraphqLDto>();
            return Task.FromResult(result);
        }
    }
}
