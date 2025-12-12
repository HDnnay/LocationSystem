using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Patients.MapperExtensions;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Queries.GetPatienList
{
    public class GetPatienListQueryHandler : IRequestHandler<GetPatienListQuery, List<PatienListDto>>
    {
        private readonly IPatientRepository _patientRepository;
        public GetPatienListQueryHandler(IPatientRepository repository)
        {
            _patientRepository = repository;
        }
        public async Task<List<PatienListDto>> Handle(GetPatienListQuery request)
        {
            var patients =await _patientRepository.GetAll();
            return patients.Select(t=> t.MapToPatienListDto()).ToList();
        }
    }
}
