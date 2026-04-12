using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.Menus;
using AutoMapper;

namespace LocationSystem.Application.Features.Menus.Commands.CreateMenu
{
    public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateMenuCommandHandler(IMenuRepository menuRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MenuDto> Handle(CreateMenuCommand command)
        {
            var menu = new Menu(
                command.Name,
                command.Path,
                command.Icon,
                command.Order,
                command.Level,
                command.ParentId
            );
            await _unitOfWork.BeginTransactionAsync();
            var createdMenu = await _menuRepository.AddAsync(menu);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<MenuDto>(createdMenu);
        }
    }
}
