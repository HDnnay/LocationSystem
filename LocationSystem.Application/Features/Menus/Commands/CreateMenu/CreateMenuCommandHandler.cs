using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Menus.Commands.CreateMenu
{
    public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;

        public CreateMenuCommandHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<MenuDto> Handle(CreateMenuCommand command)
        {
            var menu = new Menu(
                command.Name,
                command.Path,
                command.Icon,
                command.Order,
                command.ParentId
            );
            var createdMenu = await _menuRepository.AddAsync(menu);
            return new MenuDto
            {
                Id = createdMenu.Id,
                Name = createdMenu.Name,
                Path = createdMenu.Path,
                Icon = createdMenu.Icon,
                Order = createdMenu.Order,
                ParentId = createdMenu.ParentId
            };
        }
    }
}
