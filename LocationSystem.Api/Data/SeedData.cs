using LocationSystem.Domain.Entities;
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
            var permissions = new List<Permission>
            {
                new Permission("牙医管理", "dentist:view", "查看牙医信息"),
                new Permission("牙医创建", "dentist:create", "创建牙医信息"),
                new Permission("牙医编辑", "dentist:edit", "编辑牙医信息"),
                new Permission("牙医删除", "dentist:delete", "删除牙医信息"),
                new Permission("患者管理", "patient:view", "查看患者信息"),
                new Permission("患者创建", "patient:create", "创建患者信息"),
                new Permission("患者编辑", "patient:edit", "编辑患者信息"),
                new Permission("患者删除", "patient:delete", "删除患者信息"),
                new Permission("牙科诊所管理", "dental-office:view", "查看牙科诊所信息"),
                new Permission("牙科诊所创建", "dental-office:create", "创建牙科诊所信息"),
                new Permission("牙科诊所编辑", "dental-office:edit", "编辑牙科诊所信息"),
                new Permission("牙科诊所删除", "dental-office:delete", "删除牙科诊所信息"),
                new Permission("预约管理", "appointment:view", "查看预约信息"),
                new Permission("预约创建", "appointment:create", "创建预约信息"),
                new Permission("预约编辑", "appointment:edit", "编辑预约信息"),
                new Permission("预约删除", "appointment:delete", "删除预约信息"),
                new Permission("角色管理", "role:view", "查看角色信息"),
                new Permission("角色创建", "role:create", "创建角色信息"),
                new Permission("角色编辑", "role:edit", "编辑角色信息"),
                new Permission("角色删除", "role:delete", "删除角色信息"),
                new Permission("权限管理", "permission:view", "查看权限信息"),
                new Permission("权限创建", "permission:create", "创建权限信息"),
                new Permission("权限编辑", "permission:edit", "编辑权限信息"),
                new Permission("权限删除", "permission:delete", "删除权限信息"),
                new Permission("公司管理", "company:view", "查看公司信息"),
                new Permission("统计管理", "company:statistics:view", "查看统计信息"),
                new Permission("租房管理", "rent:view", "查看租房信息"),
                new Permission("租房创建", "rent:create", "创建租房信息")
            };

            foreach (var permission in permissions)
            {
                if (!await dbContext.Permissions.AnyAsync(p => p.Code == permission.Code))
                {
                    await dbContext.Permissions.AddAsync(permission);
                }
            }
            await dbContext.SaveChangesAsync();

            // 2. 创建所有必要的菜单
            var menus = new List<Menu>
            {
                new Menu("牙医管理", "/dentists", "UserFilled", 1),
                new Menu("患者管理", "/patients", "User", 2),
                new Menu("牙科诊所管理", "/dental-offices", "OfficeBuilding", 3),
                new Menu("预约管理", "/appointments", "Calendar", 4),
                new Menu("角色权限管理", "/roles", "Lock", 5),
                new Menu("角色管理", "/roles", "SetUp", 1, (await dbContext.Menus.FirstOrDefaultAsync(m => m.Path == "/roles"))?.Id),
                new Menu("权限管理", "/permissions", "Key", 2, (await dbContext.Menus.FirstOrDefaultAsync(m => m.Path == "/roles"))?.Id),
                new Menu("公司管理", "/company", "OfficeBuilding", 6),
                new Menu("公司列表", "/company", "List", 1, (await dbContext.Menus.FirstOrDefaultAsync(m => m.Path == "/company"))?.Id),
                new Menu("统计管理", "/company/provice", "List", 2, (await dbContext.Menus.FirstOrDefaultAsync(m => m.Path == "/company"))?.Id),
                new Menu("租房管理", "/rent", "House", 7),
                new Menu("租房列表", "/rent", "List", 1, (await dbContext.Menus.FirstOrDefaultAsync(m => m.Path == "/rent"))?.Id),
                new Menu("租房创建", "/rent/create", "Document", 2, (await dbContext.Menus.FirstOrDefaultAsync(m => m.Path == "/rent"))?.Id)
            };

            foreach (var menu in menus)
            {
                if (!await dbContext.Menus.AnyAsync(m => m.Path == menu.Path))
                {
                    await dbContext.Menus.AddAsync(menu);
                }
            }
            await dbContext.SaveChangesAsync();

            // 3. 建立权限和菜单的关联关系
            var permissionMenuMappings = new List<(string PermissionCode, string MenuPath)>
            {
                ("dentist:view", "/dentists"),
                ("patient:view", "/patients"),
                ("dental-office:view", "/dental-offices"),
                ("appointment:view", "/appointments"),
                ("role:view", "/roles"),
                ("role:view", "/roles"),
                ("permission:view", "/permissions"),
                ("company:view", "/company"),
                ("company:view", "/company"),
                ("company:statistics:view", "/company/provice"),
                ("rent:view", "/rent"),
                ("rent:view", "/rent"),
                ("rent:create", "/rent/create")
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
                adminRole = new Role("超级管理员", "admin", "系统超级管理员，拥有所有权限");
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
            var adminEmail = new Email("admin@example.com");
            var adminUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
            if (adminUser == null)
            {
                adminUser = new Dentist("超级管理员", adminEmail, "ADMIN001");
                adminUser.SetPasswordHash("Admin123!");
                await dbContext.Users.AddAsync(adminUser);
                await dbContext.SaveChangesAsync();

                // 7. 将超级管理员角色分配给超级管理员用户
                adminUser.AddRole(adminRole);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
