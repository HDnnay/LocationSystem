using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LocationSystem.Application.Features.Companys.Commands.ProcessCompanyData
{
    public class ProcessCompanyDataHandler : IRequestHandler<ProcessCompanyDataCommand>
    {
        private readonly ICompanyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessCompanyDataHandler(ICompanyRepository repository,IUnitOfWork unitOfWork)
        {
            _repository=repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ProcessCompanyDataCommand request)
        {
            try
            {
                var datas = new List<Company>();
                foreach (var item in request.Data)
                {
                    var company = new Company
                    {
                        Id = item.Id,
                        Name = item.CompanyName,
                        Address = item.Address,
                        PhoneNumber = item.PhoneNumber
                    };
                    datas.Add(company);
                }
                await _unitOfWork.BeginTransactionAsync();
                await _repository.AddRangAsync(datas);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex) 
            {
                throw;
            }
            
        }
    }
}
