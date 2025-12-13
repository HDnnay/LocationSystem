using FluentValidation;
using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentRepository _repository;
        private readonly IValidator<CreateAppointmentCommand> _validator;
        public CreateAppointmentCommandHandler(IAppointmentRepository repository, IUnitOfWork unitOfWork,
            IValidator<CreateAppointmentCommand> validator) 
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<Guid> Handle(CreateAppointmentCommand request)
        {
            var validationResult= await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new CustomVallidatorException(validationResult);
            var appointment = new Appointment();
        }
    }
}
