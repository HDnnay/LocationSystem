using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Patients.MapperExtensions;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Queries.GetPatienList
{
    public class GetPatienListQueryHandler : IRequestHandler<GetPatienListQuery, PageResult<PatienListDto>>
    {
        private readonly IPatientRepository _patientRepository;
        public GetPatienListQueryHandler(IPatientRepository repository)
        {
            _patientRepository = repository;
        }
        public async Task<PageResult<PatienListDto>> Handle(GetPatienListQuery request)
        {
            var patients =await _patientRepository.GetAll();
            var total= await _patientRepository.GetTotalCount();
            return new PageResult<PatienListDto>
            {
                Data = patients.Select(t => t.MapToPatienListDto()).ToList(),
                Total = total
                
            };
        }
    }
}
