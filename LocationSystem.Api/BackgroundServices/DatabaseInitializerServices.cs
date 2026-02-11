using LocationSystem.Application.Features.Companys.Commands.ProcessCompanyData;
using LocationSystem.Application.Utilities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LocationSystem.Api.BackgroudServices
{
    public class DatabaseInitializerServices : BackgroundService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseInitializerServices> _logger;
        private long lastId = 0;
        bool hasMoreData = true;
        private readonly IServiceScopeFactory _scopeFactory;

        public DatabaseInitializerServices(IConfiguration configuration,
             IServiceScopeFactory scopeFactory,
            ILogger<DatabaseInitializerServices> logger)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _logger = logger;
            // 获取SQLite连接字符串
            var sqliteConnectionString = configuration.GetConnectionString("SqliteConnectionString") ?? throw new ArgumentNullException("SqliteConnectionString");
            
            // 检查连接字符串是否使用相对路径
            if (sqliteConnectionString.Contains("./sqlite/test.db"))
            {
                // 在容器中，使用绝对路径
                _connectionString = sqliteConnectionString.Replace("./sqlite/test.db", "./Sqlite/test.db");
                _logger.LogInformation($"使用SQLite连接字符串: {_connectionString}");
            }
            else
            {
                _connectionString = sqliteConnectionString;
            }
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
            _logger.LogInformation($"开始处理公司数据，每批 {pageSize} 条");
            while (hasMoreData && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 查询一批数据
                    var companies = await GetCompaniesBatchAsync(lastId, pageSize);
                    await ProcessBatchAsync(companies, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "处理公司数据时发生错误:"+ex.Message);
                }
            }
        }

        private async Task<List<CompanyModel>> GetCompaniesBatchAsync(long lastId, int batchSize)
        {
            var companies = new List<CompanyModel>();

            try
            {
                // 确保SQLite数据库文件所在的目录存在
                var sqlitePath = Path.Combine(Directory.GetCurrentDirectory(), "Sqlite");
                if (!Directory.Exists(sqlitePath))
                {
                    Directory.CreateDirectory(sqlitePath);
                    _logger.LogInformation($"创建了SQLite数据库目录: {sqlitePath}");
                }
                
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
                    companies.Add(new CompanyModel
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

        private async Task ProcessBatchAsync(List<CompanyModel> companies, CancellationToken stoppingToken)
        {

            try
            {
                List<ProcessCompanyDataDto> dtos = new List<ProcessCompanyDataDto>();
                foreach (var item in companies)
                {
                    dtos.Add(item.ToDto());
                }
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await _mediator.Send(new ProcessCompanyDataCommand { Data = dtos });
                }

                // 这里写你的处理逻辑
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // 继续处理下一条，不因为单条失败而停止
            }
            // 每条记录处理间隔（避免过快）
            await Task.Delay(10, stoppingToken);
        }

        private async Task ProcessSingleCompanyAsync(CompanyModel company)
        {
            // 这里写你的具体业务逻辑
            // 例如：发送到其他系统、数据转换、验证等
            await Task.Delay(50); // 模拟处理时间
            // _logger.LogDebug($"处理公司: {company.Name}");
        }
    }

    public class CompanyModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public ProcessCompanyDataDto ToDto()
        {
            return new ProcessCompanyDataDto
            {
                Id = Guid.NewGuid(),
                CompanyName = this.Name,
                Address = this.Address,
                PhoneNumber = this.Phone
            };

        }
    }

}
