using FluentValidation;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Dentists.Commands.CreateDentist
{
    public class CreateDentistCommandHandler : IRequestHandler<CreateDentistCommand, Guid>
    {
        private readonly IDentistRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateDentistCommand> _validator;
        public CreateDentistCommandHandler(IDentistRepository repository,IUnitOfWork unitOfWork,
            IValidator<CreateDentistCommand> validator) 
        {
            _repository = repository;
            _unitOfWork = unitOfWork;   
            _validator = validator; 
        }
        public async Task<Guid> Handle(CreateDentistCommand request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid) 
            {
                throw new CustomVallidatorException(validationResult);
            }
            var model = new Dentist(request.Name,new Email(request.Email));
            try
            {
                await _repository.AddAsync(model);
                await _unitOfWork.Commit();
                return model.Id;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                throw;
            }


        }
    }
}
