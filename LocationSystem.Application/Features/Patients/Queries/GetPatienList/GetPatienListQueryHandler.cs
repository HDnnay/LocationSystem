using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Patients.Mapper;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Queries.GetPatienList
{
    public class GetPatienListQueryHandler : IRequsetHandler<GetPatienListQuery, PageResult<PatienListDto>>
    {
        private readonly IPatientRepository _patientRepository;
        public GetPatienListQueryHandler(IPatientRepository repository)
        {
            _patientRepository = repository;
        }
        public async Task<PageResult<PatienListDto>> Handle(GetPatienListQuery request)
        {
            var patients =await _patientRepository.GetPatientPage(request);
            var total= await _patientRepository.GetTotalCount();
            return new PageResult<PatienListDto>
            {
                Data = patients.Select(t => t.MapToPatienListDto()).ToList(),
                Total = total
                
            };
        }
    }
}
