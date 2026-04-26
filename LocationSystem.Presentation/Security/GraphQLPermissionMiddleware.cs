using HotChocolate.Resolvers;
using LocationSystem.Core;
using LocationSystem.Core.Security.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation.Security
{
    public class GraphQLPermissionMiddleware
    {
        private readonly FieldDelegate _next;
        private readonly string _permissionCode;

        public GraphQLPermissionMiddleware(FieldDelegate next, string permissionCode)
        {
            _next = next;
            _permissionCode = permissionCode;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            // 1. 从 GraphQL 上下文中提取用户信息
            var userId = ExtractUserIdFromGraphQLContext(context);

            if (userId == null)
            {
                throw new GraphQLException("用户未认证");
            }

            // 2. 使用 Core 层的验证服务
            var validator = context.Services.GetRequiredService<IPermissionValidator>();
            var validationContext = new PermissionValidationContext
            {
                UserId = userId.Value,
                PermissionCode = _permissionCode,
                // 其他上下文信息...
            };

            var result = await validator.ValidateAsync(validationContext);

            // 3. 根据验证结果处理
            if (!result.IsAuthorized)
            {
                throw new GraphQLException(result.IsSuperAdmin ?
                    "权限配置错误" : "权限不足");
            }

            // 4. 继续执行下一个中间件
            await _next(context);
        }

        private Guid? ExtractUserIdFromGraphQLContext(IMiddlewareContext context)
        {
            // 实现从 GraphQL 上下文提取用户ID的逻辑
            // 可能从 JWT token 或 HttpContext 中获取
            return context.GetGlobalState<Guid?>("UserId");
        }
    }
}
