using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Dentists.Queries.GetDentistList
{
    public class GetDentistListQueryHandler : IRequsetHandler<GetDentistListQuery, PageResult<DentistListDto>>
    {
        private readonly IDentistRepository _repository;
        public GetDentistListQueryHandler(IDentistRepository repository) 
        {
            _repository = repository;
        }
        public async Task<PageResult<DentistListDto>> Handle(GetDentistListQuery request)
        {
            var model =await _repository.GetDentistPage(request);
            var count = await _repository.GetTotalCount();
            return new PageResult<DentistListDto>()
            {
                Data = model.Select(t=>new DentistListDto() { Id = t.Id,Name = t.Name,Email = t.Email.Value}).ToList(),
                Total = count
            };
        }
    }
}
