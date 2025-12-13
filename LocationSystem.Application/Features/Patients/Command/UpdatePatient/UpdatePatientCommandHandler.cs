using FluentValidation;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Command.UpdatePatient
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdatePatientCommand> _validator;

        public UpdatePatientCommandHandler(IPatientRepository repository, IUnitOfWork unitOfWork,
            IValidator<UpdatePatientCommand> validator)
        {
            _patientRepository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task Handle(UpdatePatientCommand request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new CustomVallidatorException(validationResult);
            var patient = await _patientRepository.GetByIdAsync(request.Id);
            if (patient == null)
                throw new ArgumentException("编辑的Patient为空");
            patient.UpdateName(request.Name);
            patient.UpdateEmail(new Email(request.Email));
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _patientRepository.UpdateAsync(patient);
                await _unitOfWork.CommitAsync();

            }
            catch (Exception) 
            {
                await _unitOfWork.RollbackAsync();
                throw;
            
            }
        }
    }
}
