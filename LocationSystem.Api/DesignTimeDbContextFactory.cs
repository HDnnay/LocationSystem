using LocationSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LocationSystem.Api
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // 使用 Microsoft.Extensions.Configuration.Json 包
            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "../LocationSystem.Api"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection")?? throw new ArgumentNullException("数据库链接字符串不存在！");
            if (configuration["DataBaseProvider"]=="Mysql")
                optionsBuilder.UseMySQL(connectionString);
            else
                optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}