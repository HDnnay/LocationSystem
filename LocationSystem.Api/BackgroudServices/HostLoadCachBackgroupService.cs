
using LocationSystem.Application.Contrats.Repositories;
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
            var model = await cachService.GetOrCreateAsync("count_provice", async _ =>
           {
               var data = await companyRepository.GetAll();
               var tastkResult = await Task.Run(async () =>
               {
                   var matchedResults = new List<ProviceCompanyModel>();
                   foreach (var item in data)
                   {
                       foreach (var item2 in ProvinceDataExtentions.ReverseProvinceMap)
                       {
                           if (item.Address.StartsWith(item2.Key))
                           {
                               matchedResults.Add(new ProviceCompanyModel { Id = item.Id, Name = item.Name, Address = item.Address, Provice = item2.Key });
                           }
                       }
                   }
                   return matchedResults;
               });
               var result = from item in tastkResult
                            group item by item.Provice into provinceGroup
                            let count = provinceGroup.Count()
                            orderby count descending
                            select new Dictionary<string, int>() { { provinceGroup.Key, count } };
               return new GetProviceCompanyDto() { ProviceConpany = result.ToList() };
           }, 600);

            #endregion
        }
    }
}
