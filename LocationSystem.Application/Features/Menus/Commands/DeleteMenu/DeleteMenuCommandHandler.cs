using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Menus.Commands.DeleteMenu
{
    public class DeleteMenuCommandHandler : IRequestHandler<DeleteMenuCommand, bool>
    {
        private readonly IMenuRepository _menuRepository;

        public DeleteMenuCommandHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<bool> Handle(DeleteMenuCommand command)
        {
            var model = await _menuRepository.GetByIdAsync(command.MenuId);
            if (model==null)
                throw new NotFoundException("�ò˵�������");
            await _menuRepository.DeleteAsync(model);
            return true;
        }
    }
}
