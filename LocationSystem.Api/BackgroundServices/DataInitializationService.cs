using LocationSystem.Api.Data;
using LocationSystem.Domain.Entities;
using LocationSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LocationSystem.Api.BackgroudServices
{
    public class DataInitializationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DataInitializationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 等待应用启动完成
            await Task.Delay(3000, stoppingToken);
            try
            {
                Console.WriteLine("开始后台数据初始化...");
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    // 检查是否已有超级管理员用户
                    var hasAdminUser = await dbContext.Users.AnyAsync(u => u.UserType==UserType.Admin, stoppingToken);
                    if (!hasAdminUser)
                    {
                        // 执行应用初始化
                        // 初始化超级管理员账号和角色
                        Console.WriteLine("正在初始化超级管理员账号和角色...");
                        await SeedData.InitializeAsync(dbContext);
                        Console.WriteLine("超级管理员账号和角色初始化完成");
                        Console.WriteLine("后台数据初始化完成");
                    }
                    else
                    {
                        Console.WriteLine("数据库中已存在超级管理员，跳过初始化");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"后台数据初始化失败: {ex.Message}");
            }
        }
    }
}
