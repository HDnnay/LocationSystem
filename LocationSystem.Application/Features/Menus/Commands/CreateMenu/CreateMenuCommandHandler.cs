using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos.Menus;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Menus;
using Mapster;

namespace LocationSystem.Application.Features.Menus.Commands.CreateMenu
{
    public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMenuCommandHandler(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
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
                command.Level,
                command.ParentId,
                command.IsBackEnd
            );
            await _unitOfWork.BeginTransactionAsync();
            var createdMenu = await _menuRepository.AddAsync(menu);
            await _unitOfWork.CommitAsync();
            return createdMenu.Adapt<MenuDto>();
        }
    }
}
