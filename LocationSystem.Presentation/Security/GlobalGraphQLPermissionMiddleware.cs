// LocationSystem.Presentation/Security/GlobalGraphQLPermissionMiddleware.cs
using HotChocolate.Resolvers;
using LocationSystem.Application.Utilities;
using LocationSystem.Core;
using LocationSystem.Core.Security.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LocationSystem.Presentation.Security
{
    public class GlobalGraphQLPermissionMiddleware
    {
        private readonly FieldDelegate _next;
        private readonly ILogger<GlobalGraphQLPermissionMiddleware> _logger;

        public GlobalGraphQLPermissionMiddleware(
            FieldDelegate next,
            ILogger<GlobalGraphQLPermissionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            // 1. 检查是否需要权限验证
            var permissionCode = GetPermissionCodeFromContext(context);
            if (string.IsNullOrEmpty(permissionCode))
            {
                await _next(context);
                return;
            }

            // 2. 执行权限验证
            var validator = context.Services.GetRequiredService<IPermissionValidator>();
            var userId = ExtractUserId(context);

            if (userId.HasValue)
            {
                var validationContext = new PermissionValidationContext
                {
                    UserId = userId.Value,
                    PermissionCode = permissionCode
                };

                var result = await validator.ValidateAsync(validationContext);

                if (!result.IsAuthorized)
                {
                    throw new GraphQLException(result.FailureMessage);
                }
            }

            await _next(context);
        }

        private string GetPermissionCodeFromContext(IMiddlewareContext context)
        {
            // 从字段特性或配置中获取权限代码
            var field = context.Selection.Field;

            // 可以检查字段的上下文数据或特性
            if (field.ContextData.TryGetValue("RequiredPermission", out var permissionObj))
            {
                return permissionObj as string;
            }

            return null;
        }

        private Guid? ExtractUserId(IMiddlewareContext context)
        {
            var httpContext = context.GetGlobalState<HttpContext>("HttpContext");
            return httpContext?.User?.GetUserId();
        }
    }
}