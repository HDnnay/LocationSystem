using LocationSystem.Application.Contrats.Repositories;
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
                        foreach (var item2 in ProvinceData.ReverseProvinceMap)
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
                             select new Tuple<string, int>(provinceGroup.Key, provinceGroup.Count());
                return new GetProviceCompanyDto() { ProviceConpany = result.ToList() };
            },600);
            return model!;
        }
    }
    public class ProvinceData
    {
        public static Dictionary<string, string> ProvinceMap { get; } = new(){
                { "北京", "北京市" },
                { "天津", "天津市" },
                { "上海", "上海市" },
                { "重庆", "重庆市" },
                { "河北", "河北省" },
                { "山西", "山西省" },
                { "辽宁", "辽宁省" },
                { "吉林", "吉林省" },
                { "黑龙江", "黑龙江省" },
                { "江苏", "江苏省" },
                { "浙江", "浙江省" },
                { "安徽", "安徽省" },
                { "福建", "福建省" },
                { "江西", "江西省" },
                { "山东", "山东省" },
                { "河南", "河南省" },
                { "湖北", "湖北省" },
                { "湖南", "湖南省" },
                { "广东", "广东省" },
                { "海南", "海南省" },
                { "四川", "四川省" },
                { "贵州", "贵州省" },
                { "云南", "云南省" },
                { "陕西", "陕西省" },
                { "甘肃", "甘肃省" },
                { "青海", "青海省" },
                { "台湾", "台湾省" },
                { "内蒙古", "内蒙古自治区" },
                { "广西", "广西壮族自治区" },
                { "西藏", "西藏自治区" },
                { "宁夏", "宁夏回族自治区" },
                { "新疆", "新疆维吾尔自治区" },
                { "香港", "香港特别行政区" },
                { "澳门", "澳门特别行政区" }
        };

        // 反向映射：从完整名称到简称
        public static Dictionary<string, string> ReverseProvinceMap { get; } =
            ProvinceMap.ToDictionary(pair => pair.Value, pair => pair.Key);

        // 正向查找：简称 -> 完整名称
        public static string GetFullName(string shortName)
        {
            return ProvinceMap.TryGetValue(shortName, out var fullName)
                ? fullName
                : shortName;
        }

        // 反向查找：完整名称 -> 简称
        public static string GetShortName(string fullName)
        {
            return ReverseProvinceMap.TryGetValue(fullName, out var shortName)
                ? shortName
                : fullName;
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
