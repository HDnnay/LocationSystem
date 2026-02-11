using LocationSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Api.Data
{
    public class DatabaseMigrator
    {
        public static async Task MigrateAsync(AppDbContext dbContext)
        {
            int maxRetries = 5;
            int retryDelay = 5000; // 5秒
            
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    Console.WriteLine($"🔄 尝试数据库迁移 (尝试 {i+1}/{maxRetries})...");

                    // 直接执行迁移，确保所有迁移都被应用
                    Console.WriteLine("正在执行数据库迁移...");
                    await dbContext.Database.MigrateAsync();

                    Console.WriteLine("✅ 数据库迁移完成");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ 数据库迁移失败: {ex.Message}");
                    if (i < maxRetries - 1)
                    {
                        Console.WriteLine($"⏳ 等待 {retryDelay/1000} 秒后重试...");
                        await Task.Delay(retryDelay);
                    }
                }
            }
        }
    }
}
