using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Menus.Commands.UpdateMenu
{
    public class UpdateMenuCommand : IRequest<MenuDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
    }
}
