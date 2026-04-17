using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using Mapster;

namespace LocationSystem.Application.Features.Menus.Commands.UpdateMenu
{
    public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;

        public UpdateMenuCommandHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
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
                command.ParentId,
                command.IsBackEnd
            );

            await _menuRepository.UpdateAsync(menu);
            return menu.Adapt<MenuDto>();
        }
    }
}
