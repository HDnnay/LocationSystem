using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Interfaces;
using LocationSystem.Application.Extentions;
using LocationSystem.Application.Features.Companys.Queries.GetProviceCompany;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Companys.Queries.GetProviceConpany
{
    public class GetProviceCompanyQueryHandler : IRequestHandler<GetProviceCompanyQuery, GetProviceCompanyDto>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICacheService _cacheService;
        public GetProviceCompanyQueryHandler(ICompanyRepository repository, ICacheService cacheService)
        {
            _companyRepository = repository;
            _cacheService = cacheService;
        }
        public async Task<GetProviceCompanyDto> Handle(GetProviceCompanyQuery request)
        {
            var model =await _cacheService.GetOrCreateAsync(CacheKeys.ProvinceCompanyCount, async _ =>
            {
               
                var data = await _companyRepository.GetAllFromSelectedFields(u => new ProvinceDto
                {
                    Address = u.Address,
                    Province = u.Province
                });

                if (data.Any()&&!string.IsNullOrWhiteSpace(data.FirstOrDefault().Province))
                {
                   return GroupProvinceCompany(data);
                }
                else
                {
                    var tastkResult = await Task.Run(async () =>
                    {
                        var matchedResults = new List<ICompanyEntity>();
                        foreach (var item in data)
                        {
                            foreach (var item2 in ProvinceDataExtentions.ReverseProvinceMap)
                            {
                                if (item.Address.StartsWith(item2.Key))
                                {
                                    matchedResults.Add(new BasiceCompnay {Address = item.Address, Province = item2.Key });
                                }
                            }
                        }
                        return matchedResults;
                    });
                    return GroupProvinceCompany(tastkResult);
                }


            },600);
            return model!;
        }

        private static GetProviceCompanyDto? GroupProvinceCompany<T>(IEnumerable<T> tastkResult) where T:ICompanyEntity
        {
            var result = from item in tastkResult
                         group item by item.Province into provinceGroup
                         let count = provinceGroup.Count()
                         orderby count descending
                         select new Dictionary<string, int>() { { provinceGroup.Key, count } };
            return new GetProviceCompanyDto() { ProviceConpany = result.ToList() };
        }
    }
    public class ProviceCompanyModel:ICompanyEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? Province { get; set; }
    }
    public class BasiceCompnay : ICompanyEntity
    {
        public string? Address { get; set; }
        public string? Province { get; set; }
    }

}
