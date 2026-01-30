using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace LocationSystem.Api.BackgroudServices
{
    public class DatabaseInitializerServices : BackgroundService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseInitializerServices> _logger;
        private long lastId = 0;
        bool hasMoreData = true;

        public DatabaseInitializerServices(IConfiguration configuration,
            ILogger<DatabaseInitializerServices> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("SqliteConnectionString");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("公司数据处理服务启动");

            // 等待应用启动完成
            await Task.Delay(3000, stoppingToken);
            try
            {
                await ProcessAllCompaniesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理公司数据时发生错误");
            }

            _logger.LogInformation("公司数据处理服务完成");
        }

        private async Task ProcessAllCompaniesAsync(CancellationToken stoppingToken)
        {
            int pageSize = 1000;
            int totalProcessed = 0;
            _logger.LogInformation($"开始处理公司数据，每批 {pageSize} 条");
            while (hasMoreData && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 查询一批数据
                    var companies = await GetCompaniesBatchAsync(lastId, pageSize);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "处理公司数据时发生错误:"+ex.Message);
                }
            }
        }

        private async Task<List<Company>> GetCompaniesBatchAsync(long lastId, int batchSize)
        {
            var companies = new List<Company>();

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                var command = connection.CreateCommand();

                // 简单的分页查询，按ID排序
                command.CommandText = @"
                    SELECT Id, Name, Address, Phone
                    FROM Company
                    WHERE Id > @LastId
                    ORDER BY Id
                    LIMIT @BatchSize";

                command.Parameters.AddWithValue("@LastId", lastId);
                command.Parameters.AddWithValue("@BatchSize", batchSize);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    companies.Add(new Company
                    {
                        Id = reader.GetInt64(0),
                        Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Address = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Phone = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                    });
                }
                if (companies.Any())
                    this.lastId = companies.Last().Id;
                if (companies.Any())
                    hasMoreData=!(companies.Count()<batchSize);
                else if (!companies.Any())
                    hasMoreData = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询公司数据时出错");
                throw;
            }

            return companies;
        }

        private async Task ProcessBatchAsync(List<Company> companies, CancellationToken stoppingToken)
        {
            foreach (var company in companies)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                try
                {
                    // 这里写你的处理逻辑
                    await ProcessSingleCompanyAsync(company);

                    _logger.LogDebug($"处理成功: {company.Name} (ID: {company.Id})");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"处理公司失败: {company.Name} (ID: {company.Id})");
                    // 继续处理下一条，不因为单条失败而停止
                }

                // 每条记录处理间隔（避免过快）
                await Task.Delay(10, stoppingToken);
            }
        }

        private async Task ProcessSingleCompanyAsync(Company company)
        {
            // 这里写你的具体业务逻辑
            // 例如：发送到其他系统、数据转换、验证等

            await Task.Delay(50); // 模拟处理时间
            // _logger.LogDebug($"处理公司: {company.Name}");
        }
    }

    public class Company
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }

}
