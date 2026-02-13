
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Interfaces;
using LocationSystem.Application.Extentions;
using LocationSystem.Application.Features.Companys.Queries.GetProviceConpany;
using LocationSystem.Application.Utilities;
using Microsoft.OpenApi;

namespace LocationSystem.Api.BackgroudServices
{
    public class HostLoadCachBackgroupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CompanyUpdateBackgroundService> _logger;

        public HostLoadCachBackgroupService(
            IServiceScopeFactory scopeFactory,
            ILogger<CompanyUpdateBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            #region 后台加载统计数据到redis缓存中
            using var scope = _scopeFactory.CreateScope();
            var companyRepository = scope.ServiceProvider.GetService<ICompanyRepository>();
            var cachService = scope.ServiceProvider.GetService<ICacheService>();
            var model = await cachService.GetOrCreateAsync(CacheKeys.ProvinceCompanyCount, async _ =>
            {
                var data = await companyRepository.GetAllFromSelectedFields(t => new CompanyViewModel { Address=t.Address,Province= t.Province });
                if (data.Any()&&!string.IsNullOrWhiteSpace(data.FirstOrDefault().Province))
                {
                    return GetGroupProvince(data);
                }
                var tastkResult = await Task.Run(async () =>
                {
                    var matchedResults = new List<ICompanyEntity>();
                    foreach (var item in data)
                    {
                        foreach (var item2 in ProvinceDataExtentions.ReverseProvinceMap)
                        {
                            if (item.Address.StartsWith(item2.Key))
                            {
                                matchedResults.Add(new CompanyViewModel { Address=item.Address, Province=item2.Key });
                            }
                        }
                    }
                    return matchedResults;
                });
                return GetGroupProvince(tastkResult);
            }, 600);

            #endregion
        }

        private static GetProviceCompanyDto? GetGroupProvince(IEnumerable<ICompanyEntity> tastkResult)
        {
            var result = from item in tastkResult
                         group item by item.Province into provinceGroup
                         let count = provinceGroup.Count()
                         orderby count descending
                         select new Dictionary<string, int>() { { provinceGroup.Key, count } };
            return new GetProviceCompanyDto() { ProviceConpany = result.ToList() };
        }
    }
    public class CompanyViewModel : ICompanyEntity
    {
        public string? Address { get; set; }
        public string? Province { get; set; }
    }
}
