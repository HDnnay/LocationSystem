using LocationSystem.Domain.Entities.Articles;
using LocationSystem.Domain.Entities.Menus;
using LocationSystem.Domain.Entities.UserRolePermissions;
using LocationSystem.Domain.ValueObjects;
using LocationSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Api.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(AppDbContext dbContext)
        {
            // 1. 创建所有必要的权限
            // 先清空现有权限数据，重新创建
            dbContext.Permissions.RemoveRange(await dbContext.Permissions.ToListAsync());
            await dbContext.SaveChangesAsync();

            // 创建顶级权限
            var companyViewPermission = new Permission("公司管理", "company:view", "查看公司信息");
            var rentViewPermission = new Permission("租房管理", "rent:view", "查看租房信息");
            var roleViewPermission = new Permission("角色管理", "role:view", "查看角色信息");
            var permissionViewPermission = new Permission("权限管理", "permission:view", "查看权限信息");
            var userViewPermission = new Permission("用户管理", "user:view", "查看用户信息");
            var menuViewPermission = new Permission("菜单管理", "menu:view", "查看菜单信息");

            // 添加顶级权限到数据库
            await dbContext.Permissions.AddAsync(companyViewPermission);
            await dbContext.Permissions.AddAsync(rentViewPermission);
            await dbContext.Permissions.AddAsync(roleViewPermission);
            await dbContext.Permissions.AddAsync(permissionViewPermission);
            await dbContext.Permissions.AddAsync(userViewPermission);
            await dbContext.Permissions.AddAsync(menuViewPermission);
            await dbContext.SaveChangesAsync();

            // 创建子权限
            var permissions = new List<Permission>
            {
                new Permission("用户创建","user:create","创建用户",userViewPermission.Id),
                new Permission("用户编辑","user:edit","创建用户",userViewPermission.Id),
                new Permission("用户删除","user:delete","创建用户",userViewPermission.Id),
                new Permission("用户恢复","user:recover","恢复用户",userViewPermission.Id),
                new Permission("角色创建", "role:create", "创建角色信息", roleViewPermission.Id),
                new Permission("角色编辑", "role:edit", "编辑角色信息", roleViewPermission.Id),
                new Permission("角色删除", "role:delete", "删除角色信息", roleViewPermission.Id),
                new Permission("权限创建", "permission:create", "创建权限信息", permissionViewPermission.Id),
                new Permission("权限编辑", "permission:edit", "编辑权限信息", permissionViewPermission.Id),
                new Permission("权限删除", "permission:delete", "删除权限信息", permissionViewPermission.Id),
                new Permission("公司列表", "company:list", "查看公司列表", companyViewPermission.Id),
                new Permission("统计管理", "company:statistics:view", "查看统计信息", companyViewPermission.Id),
                new Permission("租房列表", "rent:list", "查看租房列表", rentViewPermission.Id),
                new Permission("租房创建", "rent:create", "创建租房信息", rentViewPermission.Id)
            };

            // 添加子权限到数据库
            foreach (var permission in permissions)
            {
                await dbContext.Permissions.AddAsync(permission);
            }
            await dbContext.SaveChangesAsync();

            // 2. 创建所有必要的菜单
            // 先清空现有菜单数据，重新创建
            dbContext.Menus.RemoveRange(await dbContext.Menus.ToListAsync());
            await dbContext.SaveChangesAsync();

            // 创建顶级菜单 (Level = 1)
            var rolePermissionMenu = new Menu("角色权限管理", "/admin/role-permissions", "Lock", 1, 1, null, true); // 后端菜单
            var companyMenu = new Menu("公司管理", "/admin/company", "OfficeBuilding", 2, 1, null, true); // 后端菜单
            var rentMenu = new Menu("租房管理", "/admin/rent", "House", 3, 1, null, true); // 后端菜单

            // 添加顶级菜单到数据库
            await dbContext.Menus.AddAsync(rolePermissionMenu);
            await dbContext.Menus.AddAsync(companyMenu);
            await dbContext.Menus.AddAsync(rentMenu);
            await dbContext.SaveChangesAsync();

            // 创建子菜单 (Level = 2)
            var roleMenu = new Menu("角色管理", "/admin/roles", "SetUp", 1, 2, rolePermissionMenu.Id, true); // 后端菜单
            var permissionManagementMenu = new Menu("权限管理", "/admin/permissions", "Key", 2, 2, rolePermissionMenu.Id, true); // 后端菜单
            var userManagementMenu = new Menu("用户管理", "/admin/users", "User", 3, 2, rolePermissionMenu.Id, true); // 后端菜单
            var menuManagementMenu = new Menu("菜单管理", "/admin/menus", "List", 4, 2, rolePermissionMenu.Id, true); // 后端菜单
            var companyListMenu = new Menu("公司列表", "/admin/company/list", "List", 1, 2, companyMenu.Id, true); // 后端菜单
            var companyStatisticsMenu = new Menu("统计管理", "/admin/company/statistics", "List", 2, 2, companyMenu.Id, true); // 后端菜单
            var rentListMenu = new Menu("租房列表", "/admin/rent/list", "List", 1, 2, rentMenu.Id, true); // 后端菜单
            var rentCreateMenu = new Menu("租房创建", "/admin/rent/create", "Document", 2, 2, rentMenu.Id, true); // 后端菜单

            // 添加子菜单到数据库
            await dbContext.Menus.AddAsync(roleMenu);
            await dbContext.Menus.AddAsync(permissionManagementMenu);
            await dbContext.Menus.AddAsync(userManagementMenu);
            await dbContext.Menus.AddAsync(menuManagementMenu);
            await dbContext.Menus.AddAsync(companyListMenu);
            await dbContext.Menus.AddAsync(companyStatisticsMenu);
            await dbContext.Menus.AddAsync(rentListMenu);
            await dbContext.Menus.AddAsync(rentCreateMenu);
            await dbContext.SaveChangesAsync();

            // 3. 建立权限和菜单的关联关系
            var permissionMenuMappings = new List<(string PermissionCode, string MenuPath)>
            {
                ("role:view", "/admin/roles"),
                ("permission:view", "/admin/permissions"),
                ("user:view", "/admin/users"),
                ("menu:view", "/admin/menus"),
                ("company:view", "/admin/company/list"),
                ("company:list", "/admin/company/list"),
                ("company:statistics:view", "/admin/company/statistics"),
                ("rent:view", "/admin/rent/list"),
                ("rent:list", "/admin/rent/list"),
                ("permission:view", "/admin/role-permissions"),
                ("rent:create", "/admin/rent/create")
            };

            foreach (var (permissionCode, menuPath) in permissionMenuMappings)
            {
                var permission = await dbContext.Permissions.FirstOrDefaultAsync(p => p.Code == permissionCode);
                var menu = await dbContext.Menus.FirstOrDefaultAsync(m => m.Path == menuPath);

                if (permission != null && menu != null)
                {
                    if (!await dbContext.PermissionMenus.AnyAsync(pm => pm.PermissionId == permission.Id && pm.MenuId == menu.Id))
                    {
                        var permissionMenu = new PermissionMenu(permission, menu);
                        await dbContext.PermissionMenus.AddAsync(permissionMenu);
                    }
                }
            }
            await dbContext.SaveChangesAsync();

            // 4. 创建超级管理员角色
            var adminRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "超级管理员");
            if (adminRole == null)
            {
                adminRole = new Role("超级管理员", "admin", true, "系统超级管理员，拥有所有权限");
                await dbContext.Roles.AddAsync(adminRole);
                await dbContext.SaveChangesAsync();

                // 5. 将所有权限分配给超级管理员角色
                var allPermissions = await dbContext.Permissions.ToListAsync();
                foreach (var permission in allPermissions)
                {
                    adminRole.AddPermission(permission);
                }
                await dbContext.SaveChangesAsync();
            }

            // 6. 创建超级管理员用户
            var adminEmail = new Email("admin@admin.com");
            var adminUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
            if (adminUser == null)
            {
                adminUser = new AdminUser("超级管理员", adminEmail, UserType.Admin, true);
                adminUser.SetPasswordHash("Admin123!");
                await dbContext.Users.AddAsync(adminUser);
                await dbContext.SaveChangesAsync();

                // 7. 将超级管理员角色分配给超级管理员用户
                adminUser.AddRole(adminRole);
                await dbContext.SaveChangesAsync();
            }

            // 8. 初始化文章相关数据
            await InitializeArticleDataAsync(dbContext, adminUser);
        }

        public static async Task InitializeArticleDataAsync(AppDbContext dbContext, User adminUser)
        {
            // 先清空现有文章相关数据，重新创建
            dbContext.ArticleComments.RemoveRange(await dbContext.ArticleComments.ToListAsync());
            dbContext.ArticleTags.RemoveRange(await dbContext.ArticleTags.ToListAsync());
            dbContext.Articles.RemoveRange(await dbContext.Articles.ToListAsync());
            await dbContext.SaveChangesAsync();

            // 创建标签
            var tags = new List<ArticleTag>
            {
                new ArticleTag { Id = Guid.NewGuid(), Name = "技术", IsVisiable = true, CreateTime = DateTime.Now },
                new ArticleTag { Id = Guid.NewGuid(), Name = "生活", IsVisiable = true, CreateTime = DateTime.Now },
                new ArticleTag { Id = Guid.NewGuid(), Name = "工作", IsVisiable = true, CreateTime = DateTime.Now },
                new ArticleTag { Id = Guid.NewGuid(), Name = "学习", IsVisiable = true, CreateTime = DateTime.Now },
                new ArticleTag { Id = Guid.NewGuid(), Name = "娱乐", IsVisiable = true, CreateTime = DateTime.Now }
            };

            foreach (var tag in tags)
            {
                await dbContext.ArticleTags.AddAsync(tag);
            }
            await dbContext.SaveChangesAsync();

            // 创建文章
            var articles = new List<Article>
            {
                new Article("技术文章1", "技术文章内容", true, adminUser.Id, "技术主题", "这是一篇技术文章"),
                new Article("生活文章1", "生活文章内容", true, adminUser.Id, "生活主题", "这是一篇生活文章"),
                new Article("工作文章1", "工作文章内容", true, adminUser.Id, "工作主题", "这是一篇工作文章"),
                new Article("学习文章1", "学习文章内容", true, adminUser.Id, "学习主题", "这是一篇学习文章"),
                new Article("娱乐文章1", "娱乐文章内容", true, adminUser.Id, "娱乐主题", "这是一篇娱乐文章")
            };

            foreach (var article in articles)
            {
                await dbContext.Articles.AddAsync(article);
            }
            await dbContext.SaveChangesAsync();

            // 为文章添加标签
            articles[0].UpdateTags(new List<ArticleTag> { tags[0], tags[3] });
            articles[1].UpdateTags(new List<ArticleTag> { tags[1], tags[4] });
            articles[2].UpdateTags(new List<ArticleTag> { tags[2], tags[3] });
            articles[3].UpdateTags(new List<ArticleTag> { tags[3] });
            articles[4].UpdateTags(new List<ArticleTag> { tags[4] });

            await dbContext.SaveChangesAsync();

            // 创建评论
            var comments = new List<ArticleComment>
            {
                new ArticleComment(adminUser.Id, "这是一条评论1", true, articles[0].Id),
                new ArticleComment(adminUser.Id, "这是一条评论2", true, articles[0].Id),
                new ArticleComment(adminUser.Id, "这是一条评论3", true, articles[1].Id),
                new ArticleComment(adminUser.Id, "这是一条评论4", true, articles[2].Id),
                new ArticleComment(adminUser.Id, "这是一条评论5", true, articles[3].Id)
            };

            foreach (var comment in comments)
            {
                await dbContext.ArticleComments.AddAsync(comment);
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
