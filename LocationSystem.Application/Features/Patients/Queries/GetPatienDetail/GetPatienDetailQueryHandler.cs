using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Queries.GetPatienDetail
{
    public class GetPatienDetailQueryHandler : IRequsetHandler<GetPatienDetailQuery, PatienDetailDto>
    {
        private readonly IPatientRepository _patientRepository;
        
        public GetPatienDetailQueryHandler(IPatientRepository repository) 
        {
            _patientRepository = repository;
        }
        public async Task<PatienDetailDto> Handle(GetPatienDetailQuery request)
        {
            var entity = await _patientRepository.GetByIdAsync(request.PatientId);
            if (entity == null) 
                throw new ArgumentException("查找的Patien的结果为空");
            return new PatienDetailDto() 
            {
                Id = entity.Id ,
                Name = entity.Name ,
                Email = entity.Email.Value
            };
            
        }
    }
}
