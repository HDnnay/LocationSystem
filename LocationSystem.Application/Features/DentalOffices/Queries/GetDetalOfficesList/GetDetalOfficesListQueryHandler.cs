using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList
{
    public class GetDetalOfficesListQueryHandler : IRequestHandler<GetDetalOfficesListQuery, PageResult<DentalOfficesListDto>>
    {
        private readonly IDentalOfficeRepository _repositoty;
        private readonly ICacheService _CacheService;
        public GetDetalOfficesListQueryHandler(IDentalOfficeRepository repositoty,ICacheService cacheService)
        {
            _repositoty = repositoty;
            _CacheService = cacheService;
        }
        public async Task<PageResult<DentalOfficesListDto>> Handle(GetDetalOfficesListQuery request)
        {
            var key = DentalOfficeCacheKey.DentalOfficeAllKey;
            var model = await _CacheService.GetOrCreateAsync(key, async _ =>
            {
                var result = await _repositoty.GetDentalOfficePage(request);
                return result.Select(t => t.MapToDto()).ToList();
            });
            return new PageResult<DentalOfficesListDto>()
            {
                Data = model!,
                Total = await _repositoty.GetTotalCount()
            };
        }
    }
}
