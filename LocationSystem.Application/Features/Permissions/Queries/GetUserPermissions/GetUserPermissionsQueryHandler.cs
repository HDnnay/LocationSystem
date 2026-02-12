using LocationSystem.Application.Services;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Permissions.Queries.GetUserPermissions
{
    public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, List<string>>
    {
        private readonly PermissionManagement _permissionManagement;

        public GetUserPermissionsQueryHandler(PermissionManagement permissionManagement)
        {
            _permissionManagement = permissionManagement;
        }

        public async Task<List<string>> Handle(GetUserPermissionsQuery request)
        {
            // 获取用户的所有权限代码
            return await _permissionManagement.GetUserPermissionCodesAsync(request.UserId);
        }
    }
}