﻿using LocationSystem.Presentation.Extensions;
using LocationSystem.Presentation.Models;

namespace LocationSystem.Presentation.GraphQL
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Name("Query");

            // 文章相关字段 - 添加权限中间件
            descriptor.Field(q => q.GetArticles(default!))
                .Type<ListType<ArticleType>>()
                .UsePermissionMiddleware("article.list.read")  // 字段级权限中间件
                .Description("获取文章列表，支持过滤和排序");

            // 用户相关字段 - 需要管理员权限
            descriptor.Field(q => q.GetUsers(default!))
                .Type<ListType<UserType>>()
                .RequireAllPermissions("user.list.read", "admin.access")  // 需要所有权限
                .Description("获取用户列表（需要管理员权限）");

            // 角色相关字段 - 需要管理员权限
            descriptor.Field(q => q.GetRoles(default!))
                .Type<ListType<RoleType>>()
                .RequireAllPermissions("role.list.read", "admin.access")
                .Description("获取角色列表（需要管理员权限）");

            // 权限相关字段 - 需要管理员权限
            descriptor.Field(q => q.GetPermissions(default!))
                .Type<ListType<PermissionType>>()
                .RequireAllPermissions("permission.list.read", "admin.access")
                .Description("获取权限列表（需要管理员权限）");

            // 菜单相关字段 - 需要菜单读取权限
            descriptor.Field(q => q.GetMenus(default!))
                .Type<ListType<MenuType>>()
                .RequirePermission("menu:view")
                .Description("获取菜单列表");

            // 文章标签字段 - 需要标签读取权限
            descriptor.Field(q => q.GetArticleTags(default!))
                .Type<ListType<ArticleTagType>>()
                .RequirePermission("tag.list.read")
                .Description("获取文章标签列表");

            // 文章日志字段 - 需要审计日志权限
            descriptor.Field(q => q.GetArticleLogs(default!))
                .Type<ListType<ArticleLogType>>()
                .RequireAllPermissions("log.article.read", "admin.access")
                .Description("获取文章审计日志（需要管理员权限）");

            // 可以继续为其他字段添加权限中间件...
        }
    }
}
