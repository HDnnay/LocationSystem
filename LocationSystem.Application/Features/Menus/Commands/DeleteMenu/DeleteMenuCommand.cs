using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Menus.Commands.DeleteMenu
{
    public class DeleteMenuCommand : IRequest<bool>
    {
        public Guid MenuId { get; set; }
    }
}
