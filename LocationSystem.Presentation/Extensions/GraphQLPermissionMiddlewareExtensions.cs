using HotChocolate.Resolvers;
using LocationSystem.Application.Utilities;
using LocationSystem.Core;
using LocationSystem.Core.Security.Abstractions;
using LocationSystem.Presentation.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LocationSystem.Presentation.Extensions
{
    /// <summary>
    /// GraphQL 权限中间件扩展方法
    /// </summary>
    public static class GraphQLPermissionMiddlewareExtensions
    {
        /// <summary>
        /// 为 GraphQL 字段添加权限验证中间件
        /// </summary>
        public static IObjectFieldDescriptor UsePermissionMiddleware(
            this IObjectFieldDescriptor descriptor,
            string permissionCode)
        {
            return descriptor.Use(next =>
            {
                var loggerFactory = descriptor.Extend().Context.Services.GetService<ILoggerFactory>();
                var logger = loggerFactory?.CreateLogger<GraphQLPermissionMiddleware>();

                var middleware = new GraphQLPermissionMiddleware(next, permissionCode, logger);

                // 返回一个 FieldDelegate（Func<IMiddlewareContext, Task>）
                return async (IMiddlewareContext context) => await middleware.InvokeAsync(context);
            });
        }

        /// <summary>
        /// 为字段添加权限要求并自动注册中间件
        /// </summary>
        public static IObjectFieldDescriptor RequirePermission(
            this IObjectFieldDescriptor descriptor,
            string permissionCode)
        {
            descriptor
               .UsePermissionMiddleware(permissionCode)
               .Extend()
               .OnBeforeCreate(definition =>
               {
                   definition.ContextData["RequiredPermission"] = permissionCode;
               });
            return descriptor;
        }

        /// <summary>
        /// 为字段设置多个权限要求（或逻辑）
        /// </summary>
        public static IObjectFieldDescriptor RequireAnyPermission(
            this IObjectFieldDescriptor descriptor,
            params string[] permissionCodes)
        {
            return descriptor.Use(next =>
            {
                var loggerFactory = descriptor.Extend().Context.Services.GetService<ILoggerFactory>();
                var logger = loggerFactory?.CreateLogger<GraphQLPermissionMiddleware>();

                // 返回 FieldDelegate
                return async (IMiddlewareContext context) =>
                {
                    var userId = ExtractUserId(context);
                    if (userId == null)
                    {
                        throw new GraphQLException("用户未认证");
                    }

                    var validator = context.Services.GetRequiredService<IPermissionValidator>();

                    // 检查是否满足任一权限
                    foreach (var permissionCode in permissionCodes)
                    {
                        var result = await validator.ValidateAsync(new PermissionValidationContext
                        {
                            UserId = userId.Value,
                            PermissionCode = permissionCode
                        });

                        if (result.IsAuthorized)
                        {
                            await next(context);
                            return;
                        }
                    }

                    throw new GraphQLException($"权限不足，需要以下任一权限: {string.Join(", ", permissionCodes)}");
                };
            });
        }

        /// <summary>
        /// 为字段设置多个权限要求（与逻辑）
        /// </summary>
        public static IObjectFieldDescriptor RequireAllPermissions(
            this IObjectFieldDescriptor descriptor,
            params string[] permissionCodes)
        {
            return descriptor.Use(next =>
            {
                var loggerFactory = descriptor.Extend().Context.Services.GetService<ILoggerFactory>();
                var logger = loggerFactory?.CreateLogger<GraphQLPermissionMiddleware>();

                // 返回 FieldDelegate
                return async (IMiddlewareContext context) =>
                {
                    var userId = ExtractUserId(context);
                    if (userId == null)
                    {
                        throw new GraphQLException("用户未认证");
                    }

                    var validator = context.Services.GetRequiredService<IPermissionValidator>();

                    // 检查是否满足所有权限
                    foreach (var permissionCode in permissionCodes)
                    {
                        var result = await validator.ValidateAsync(new PermissionValidationContext
                        {
                            UserId = userId.Value,
                            PermissionCode = permissionCode
                        });

                        if (!result.IsAuthorized)
                        {
                            throw new GraphQLException($"权限不足，缺少权限: {permissionCode}");
                        }
                    }

                    await next(context);
                };
            });
        }

        private static Guid? ExtractUserId(IMiddlewareContext context)
        {
            var httpContext = context.GetGlobalState<HttpContext>("HttpContext");
            return httpContext?.User?.GetUserId();
        }
    }
}