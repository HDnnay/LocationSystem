using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Events;
using LocationSystem.Application.Events.Handlers;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Menus.Commands.DeleteMenu
{
    public class DeleteMenuCommandHandler : IRequestHandler<DeleteMenuCommand, bool>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IEventBus _eventBus;
        public DeleteMenuCommandHandler(IMenuRepository menuRepository, IEventBus eventBus)
        {
            _menuRepository = menuRepository;
            _eventBus = eventBus;
        }

        public async Task<bool> Handle(DeleteMenuCommand command)
        {
            var model = await _menuRepository.GetByIdAsync(command.MenuId);
            if (model==null)
                throw new NotFoundException("删除的菜单为空");
            await _menuRepository.DeleteAsync(model);
            return true;
        }
    }
}
