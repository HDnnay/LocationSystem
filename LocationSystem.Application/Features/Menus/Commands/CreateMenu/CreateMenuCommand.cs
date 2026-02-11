using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Menus.Commands.CreateMenu
{
    public class CreateMenuCommand : IRequest<MenuDto>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
    }
}
