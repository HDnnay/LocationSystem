using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Menus.Commands.CreateMenu
{
    public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMenuCommandHandler(IMenuRepository menuRepository,IUnitOfWork unitOfWork)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.BeginTransactionAsync();
            var createdMenu = await _menuRepository.AddAsync(menu);
            await _unitOfWork.CommitAsync();
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
