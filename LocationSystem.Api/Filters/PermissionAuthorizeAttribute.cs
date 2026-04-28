using LocationSystem.Application.Utilities;
using LocationSystem.Core;
using LocationSystem.Core.Security.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LocationSystem.Api.Filters
{
    /// <summary>
    /// 权限认证过滤器
    /// </summary>
    public class PermissionAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// 权限代码
        /// </summary>
        public string PermissionCode { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="permissionCode">权限代码</param>
        public PermissionAuthorizeAttribute(string permissionCode)
        {
            PermissionCode = permissionCode;
        }

        /// <summary>
        /// 异步授权过滤器
        /// </summary>
        /// <param name="context">授权过滤器上下文</param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // 获取用户ID
            var userId = context.HttpContext.User.GetUserId();
            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var permissionValidator = context.HttpContext.RequestServices.GetRequiredService<IPermissionValidator>();
            var validationContext = new PermissionValidationContext
            {
                UserId = userId.Value,
                PermissionCode = PermissionCode
            };
            var result = await permissionValidator.ValidateAsync(validationContext);
            if (!result.IsAuthorized)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
