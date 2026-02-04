using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Extentions;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

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
            var model =await _cacheService.GetOrCreateAsync("count_provice", async _ =>
            {
                var data = await _companyRepository.GetAll();
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
                             select new Dictionary<string, int>() { { provinceGroup.Key,count} };
                return new GetProviceCompanyDto() { ProviceConpany = result.ToList() };
            },600);
            return model!;
        }
    }
    public class ProviceCompanyModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? Provice { get; set; }
    }
}
