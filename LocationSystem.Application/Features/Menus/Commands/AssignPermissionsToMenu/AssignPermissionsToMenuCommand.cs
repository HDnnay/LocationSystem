using LocationSystem.Application.Utilities;
using System.Collections.Generic;

namespace LocationSystem.Application.Features.Menus.Commands.AssignPermissionsToMenu
{
    public class AssignPermissionsToMenuCommand : IRequset
    {
        public Guid MenuId { get; set; }
        public List<Guid> PermissionIds { get; set; }
    }
}
