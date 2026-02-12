using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetUserPermissions
{
    public class GetUserPermissionsQuery : IRequest<List<string>>
    {
        public Guid UserId { get; set; }
    }
}