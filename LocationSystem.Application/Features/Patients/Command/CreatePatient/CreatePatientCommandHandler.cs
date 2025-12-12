using FluentValidation;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Features.DentalOffices.Commands.CreateDentalOffice;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Command.CreatePatient
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Patient>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IValidator<CreatePatientCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        public CreatePatientCommandHandler(IPatientRepository repository,
                                           IUnitOfWork unitOfWork,
                                           IValidator<CreatePatientCommand> validator)
        {
            _patientRepository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;

        }
        public async Task<Patient> Handle(CreatePatientCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new CustomVallidatorException(validationResult);
            }
            var patient = new Patient(command.Name, new Email(command.Email));
            try
            {
                var model = await _patientRepository.AddAsync(patient);
                await _unitOfWork.Commit();
                return model;
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
