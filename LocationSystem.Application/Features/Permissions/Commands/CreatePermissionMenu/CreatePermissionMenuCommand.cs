using LocationSystem.Application.Features.Permissions.Models;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Permissions.Commands.CreatePermissionMenu
{
    public class CreatePermissionMenuCommand : IRequest<PermissionMenuDto>
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Description { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsMenu { get; set; }
        public required string MenuPath { get; set; }
        public string? MenuIcon { get; set; }
        public int Order { get; set; }
    }
}
