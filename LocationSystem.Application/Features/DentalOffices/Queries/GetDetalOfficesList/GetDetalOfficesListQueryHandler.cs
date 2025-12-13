using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Queries.GetDetalOfficesList
{
    public class GetDetalOfficesListQueryHandler : IRequestHandler<GetDetalOfficesListQuery, List<DentalOfficesListDto>>
    {
        private readonly IDentalOfficeRepository _repositoty;
        public GetDetalOfficesListQueryHandler(IDentalOfficeRepository repositoty)
        {
            _repositoty = repositoty;
        }
        public async Task<List<DentalOfficesListDto>> Handle(GetDetalOfficesListQuery request)
        {
            var result = await _repositoty.GetAll();
            return result.Select(t => t.MapToDto()).ToList();
        }
    }
}
