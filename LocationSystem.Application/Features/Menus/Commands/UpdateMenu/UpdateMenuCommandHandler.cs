using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using AutoMapper;

namespace LocationSystem.Application.Features.Menus.Commands.UpdateMenu
{
    public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public UpdateMenuCommandHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<MenuDto> Handle(UpdateMenuCommand command)
        {
            var menu = await _menuRepository.GetByIdAsync(command.Id);
            if (menu == null)
            {
                throw new Exception("菜单不存在");
            }

            menu.Update(
                command.Name,
                command.Path,
                command.Icon,
                command.Order,
                command.Level,
                command.ParentId
            );

            await _menuRepository.UpdateAsync(menu);
            return _mapper.Map<MenuDto>(menu);
        }
    }
}
