using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Queries.GetDentalOfficesDetail
{
    public class GetDentalOfficesDetailQueryHandler : IRequestHandler<GetDentalOffcesDetailQuery, DentalOfficesDetailDto>
    {
        private readonly IDentalOfficeRepository _repositoty;
        public GetDentalOfficesDetailQueryHandler(IDentalOfficeRepository repositoty)
        {
            _repositoty = repositoty;
        }
        public async Task<DentalOfficesDetailDto> Handle(GetDentalOffcesDetailQuery request)
        {
            var dentalOffice= await _repositoty.GetByIdAsync(request.Id);
            if(dentalOffice is null)
            {
                throw new Exception("Dental Office not found");
            }
            var dto = new DentalOfficesDetailDto
            {
                Id = dentalOffice.Id,
                Name = dentalOffice.Name
            };
            return dto;
        }
    }
}
