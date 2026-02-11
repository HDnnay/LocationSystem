using LocationSystem.Infrastructure;

namespace LocationSystem.Api.Data
{
    public class ApplicationInitializer
    {
        public static async Task InitializeAsync(AppDbContext dbContext)
        {
            // 执行数据库迁移
            try
            {
                await DatabaseMigrator.MigrateAsync(dbContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 数据库迁移失败: {ex.Message}");
                throw;
            }
            
            // 初始化超级管理员账号和角色
            try
            {
                Console.WriteLine("🔄 正在初始化超级管理员账号和角色...");
                await SeedData.InitializeAsync(dbContext);
                Console.WriteLine("✅ 超级管理员账号和角色初始化完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 超级管理员初始化失败: {ex.Message}");
                throw;
            }
        }
    }
}
