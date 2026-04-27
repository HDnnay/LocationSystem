using HotChocolate.Resolvers;
using LocationSystem.Application.Utilities;
using LocationSystem.Core;
using LocationSystem.Core.Security.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LocationSystem.Presentation.Security
{
    /// <summary>
    /// GraphQL 权限验证中间件
    /// </summary>
    public class GraphQLPermissionMiddleware
    {
        private readonly FieldDelegate _next;
        private readonly string _permissionCode;
        private readonly ILogger<GraphQLPermissionMiddleware> _logger;

        public GraphQLPermissionMiddleware(
            FieldDelegate next,
            string permissionCode,
            ILogger<GraphQLPermissionMiddleware> logger)
        {
            _next = next;
            _permissionCode = permissionCode;
            _logger = logger;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            _logger.LogDebug("执行权限验证中间件: {FieldName}, Permission: {Permission}",
                context.Selection.Field.Name, _permissionCode);

            // 提取用户信息
            var userId = ExtractUserId(context);
            if (userId == null)
            {
                throw new GraphQLException("用户未登录！");
            }

            // 使用 Core 层的验证服务
            var validator = context.Services.GetRequiredService<IPermissionValidator>();
            var validationContext = new PermissionValidationContext
            {
                UserId = userId.Value,
                PermissionCode = _permissionCode
            };

            var result = await validator.ValidateAsync(validationContext);

            // 处理验证结果
            if (!result.IsAuthorized)
            {
                _logger.LogWarning("权限验证失败: UserId={UserId}, Permission={Permission}",
                    userId, _permissionCode);
                throw new GraphQLException(result.FailureMessage);
            }

            _logger.LogDebug("权限验证通过: UserId={UserId}, Permission={Permission}",
                userId, _permissionCode);

            await _next(context);
        }

        private Guid? ExtractUserId(IMiddlewareContext context)
        {
            //var httpContext = context.GetGlobalState<HttpContext>("HttpContext");
            //return httpContext?.User?.GetUserId();
            var scope = context.RequestServices.CreateScope();
            var httpContextAsscessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAsscessor?.HttpContext;
            return httpContext?.User?.GetUserId();
        }
    }
}