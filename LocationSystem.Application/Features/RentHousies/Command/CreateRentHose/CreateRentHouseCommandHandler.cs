using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.RentHousies.Command.CreateRentHose
{
    public class CreateRentHouseCommandHandler : IRequestHandler<CreateRentHouseCommand>
    {
        private readonly IRentHouseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateRentHouseCommandHandler(IRentHouseRepository repository,IUnitOfWork unitOfWork) 
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(CreateRentHouseCommand request)
        {
            try
            {
                var model = request.Model;
                await _unitOfWork.BeginTransactionAsync();
                var result = await _repository.AddAsync(new RentHouse(model.Title, model.Address, model.Description, model.MonthlyRent, model.Deposit, model.Type, Guid.NewGuid(), model.Phone));
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex) 
            {
                throw;
            }
            
        }
    }
}
