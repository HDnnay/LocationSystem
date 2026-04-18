using LocationSystem.Application.Dtos.Menus;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetUserPermissionMenus
{
    public class GetUserPermissionMenusQuery : IRequest<List<PermissionMenuDto>>
    {
        public Guid UserId { get; set; }
    }
}
