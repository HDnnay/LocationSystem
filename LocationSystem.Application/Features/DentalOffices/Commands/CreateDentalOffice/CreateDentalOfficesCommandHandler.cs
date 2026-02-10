using FluentValidation;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Commands.CreateDentalOffice
{
    public class CreateDentalOfficesCommandHandler:IRequsetHandler<CreateDentalOfficesCommand, Guid>
    {
        private readonly IDentalOfficeRepository _repositoty;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateDentalOfficesCommand> _validator;
        public CreateDentalOfficesCommandHandler(IDentalOfficeRepository repositoty,
                                                IUnitOfWork unitOfWork,
                                                IValidator<CreateDentalOfficesCommand> validator)
        {
            _repositoty = repositoty;
            _unitOfWork = unitOfWork;
            _validator = validator;

        }
        public async Task<Guid> Handle(CreateDentalOfficesCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                // Handle validation failure (e.g., throw an exception or return an error response)
                throw new CustomVallidatorException(validationResult);
            }
            var dentalOffice = new DentalOffice(command.Name);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _repositoty.AddAsync(dentalOffice);
                await _unitOfWork.CommitAsync();
                return result.Id;
            }
            catch(Exception)
            {
                await _unitOfWork.RollbackAsync();
                // Log the exception (ex) here as needed
                throw; // Re-throw the exception after logging
            }
            
        }
    }
}
