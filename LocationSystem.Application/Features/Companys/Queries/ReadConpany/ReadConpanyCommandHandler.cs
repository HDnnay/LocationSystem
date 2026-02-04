using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Companys.Queries.ReadConpany
{
    public class ReadConpanyCommandHandler : IRequestHandler<ReadConpanyQuery, PageResult<CompanyDto>>
    {
        private readonly ICompanyRepository _repository;

        public ReadConpanyCommandHandler(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<PageResult<CompanyDto>> Handle(ReadConpanyQuery request)
        {
            var result =await _repository.GetCompanyPage(request);
            var pageResult = new PageResult<CompanyDto>();
            pageResult.CurrentPage = request.Page;
            foreach (var item in result)
            {
                pageResult.Total=item.Key;
                pageResult.Data = item.Value.Select(t => new CompanyDto() { Id=t.Id, Address= t.Address, Name=t.Name, PhoneNumber=t.PhoneNumber,Province=t.Province }).ToList();
            }
            return pageResult;
        }
    }
}
