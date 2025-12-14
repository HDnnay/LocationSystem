using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail
{
    public class GetDentalOfficesDetailQueryHandler : IRequestHandler<GetDentalOffcesDetailQuery, DentalOfficesDetailDto>
    {
        private readonly IDentalOfficeRepository _repositoty;
        private readonly ICacheService _cacheService;
        public GetDentalOfficesDetailQueryHandler(IDentalOfficeRepository repositoty, ICacheService cache)
        {
            _repositoty = repositoty;
            _cacheService = cache;
        }
        public async Task<DentalOfficesDetailDto?> Handle(GetDentalOffcesDetailQuery request)
        {
            try
            {
                var key = DentalOfficeCacheKey.DentalOfficeIdKey+request.Id;
                var model = await _cacheService.GetOrCreateAsync(key, async _ =>
                {
                    var dentalOffice = await _repositoty.GetByIdAsync(request.Id);
                    if (dentalOffice is null)
                    {
                        throw new Exception("Dental Office not found");
                    }
                    var dto = new DentalOfficesDetailDto
                    {
                        Id = dentalOffice.Id,
                        Name = dentalOffice.Name
                    };
                    return dto;
                });
                return model;
            }
            catch (Exception)
            {
                throw;
            }
            

        }
    }
}
