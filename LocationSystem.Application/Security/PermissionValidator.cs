using LocationSystem.Application.Services;
using LocationSystem.Application.Utilities;
using LocationSystem.Core;
using LocationSystem.Core.Security.Abstractions;
using Microsoft.Extensions.Logging;

// LocationSystem.Application/Security/PermissionValidator.cs
namespace LocationSystem.Application.Security
{
    public class PermissionValidator : IPermissionValidator
    {
        private readonly IPermissionProvider _permissionProvider;
        private readonly ICacheService _cacheService;
        private readonly ILogger<PermissionValidator> _logger;
        private readonly PermissionManagement _permissionManagement;
        public PermissionValidator(
            IPermissionProvider permissionProvider,
            ICacheService cacheService,
            PermissionManagement permissionManagement,
            ILogger<PermissionValidator> logger)
        {
            _permissionProvider = permissionProvider;
            _cacheService = cacheService;
            _permissionManagement = permissionManagement;
            _logger = logger;
        }

        public async Task<PermissionValidationResult> ValidateAsync(PermissionValidationContext context)
        {
            if (await _permissionProvider.IsSuperAdminAsync(context.UserId))
            {
                var validationResult = PermissionValidationResult.Success(true);
                return validationResult;
            }
            var codes = await _permissionProvider.GetUserPermissionCodesAsync(context.UserId);
            // 检查用户是否有权限
            if (codes.Contains(context.PermissionCode))
            {
                var result = PermissionValidationResult.Success(true);
                return result;
            }
            else
            {
                return PermissionValidationResult.Failure("认证失败没有权限");
            }
        }
    }
}
