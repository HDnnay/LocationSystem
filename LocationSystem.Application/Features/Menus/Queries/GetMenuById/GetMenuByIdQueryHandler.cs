using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using AutoMapper;

namespace LocationSystem.Application.Features.Menus.Queries.GetMenuById
{
    public class GetMenuByIdQueryHandler : IRequestHandler<GetMenuByIdQuery, MenuDto?>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetMenuByIdQueryHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<MenuDto?> Handle(GetMenuByIdQuery query)
        {
            var menu = await _menuRepository.GetByIdAsync(query.MenuId);
            if (menu == null)
            {
                return null;
            }

            return _mapper.Map<MenuDto>(menu);
        }
    }
}
