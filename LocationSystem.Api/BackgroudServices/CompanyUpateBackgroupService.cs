using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Extentions;
using LocationSystem.Application.Features.Companys.Queries.ReadConpany;
using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocationSystem.Api.BackgroudServices
{
    public class CompanyUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CompanyUpdateBackgroundService> _logger;

        public CompanyUpdateBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<CompanyUpdateBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            try
            {
                await ProcessCompaniesAsync(stoppingToken);

                // 等待一段时间再重新开始处理
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error in company update background service");
            }
        }

        private async Task ProcessCompaniesAsync(CancellationToken cancellationToken)
        {
            int page = 1;
            const int pageSize = 100; // 增大页面大小，减少数据库查询次数
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                var service = scope.ServiceProvider.GetService<ICompanyRepository>();

                if (service == null || unitOfWork == null)
                {
                    _logger.LogError("Required services not found");
                    return;
                }

                var models = await service.GetCompanyPage(new CompanyFilter() { Page = page, PageSize = pageSize });

                // 修正：检查是否有数据
                if (models == null || !models.Values.Any())
                {
                    _logger.LogInformation("No more companies to process");
                    break;
                }

                int updatedCount = 0;
                int conflictCount = 0;

                foreach (var company in models.First().Value)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    // 优化：先检查是否需要更新
                    if (string.IsNullOrEmpty(company.Address) ||
                        !string.IsNullOrEmpty(company.Province))
                        continue;

                    string foundProvince = null;

                    // 优化：使用字典查找而不是遍历
                    foreach (var item in ProvinceDataExtentions.ReverseProvinceMap)
                    {
                        if (company.Address.StartsWith(item.Key))
                        {
                            foundProvince = item.Key;
                            break; // 找到省份就退出
                        }
                    }

                    if (foundProvince == null)
                        continue;

                    try
                    {
                        // 使用单一事务处理每个公司的更新
                        await unitOfWork.BeginTransactionAsync();

                        // 重新获取最新数据，避免并发问题
                        var freshCompany = await service.GetByIdAsync(company.Id);
                        if (freshCompany == null)
                        {
                            await unitOfWork.RollbackAsync();
                            continue; // 公司已被删除
                        }

                        // 检查是否已被其他进程更新
                        if (!string.IsNullOrEmpty(freshCompany.Province))
                        {
                            await unitOfWork.RollbackAsync();
                            continue;
                        }

                        // 只更新省份字段
                        freshCompany.Province = foundProvince;
                        await service.UpdateAsync(freshCompany);

                        await unitOfWork.CommitAsync();
                        updatedCount++;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        await unitOfWork.RollbackAsync();
                        conflictCount++;

                        // 获取数据库中的当前值
                        var entry = ex.Entries.FirstOrDefault();
                        if (entry != null)
                        {
                            try
                            {
                                var databaseValues = await entry.GetDatabaseValuesAsync();
                                if (databaseValues != null)
                                {
                                    var newProvince = databaseValues.GetValue<string>(nameof(Company.Province));
                                    _logger.LogError(
                                        "Concurrency conflict for company {CompanyId}. New province: {Province}",
                                        company.Id, newProvince);
                                }
                            }
                            catch (Exception innerEx)
                            {
                                _logger.LogError(innerEx, "Error getting database values");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await unitOfWork.RollbackAsync();
                        _logger.LogWarning(ex, "Error updating company {CompanyId}", company.Id);
                    }
                }
                page++;

                // 如果当前页没有填满，说明没有更多数据了
                if (models.First().Value.Count() < pageSize)
                {
                    break;
                }
                    
            }
        }
    }
}