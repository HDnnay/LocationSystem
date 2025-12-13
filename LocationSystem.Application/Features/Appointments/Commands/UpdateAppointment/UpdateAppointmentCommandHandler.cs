using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Exceptions;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand>
    {
        private readonly IAppointmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateAppointmentCommandHandler(IAppointmentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateAppointmentCommand request)
        {
            var apppointment = await _repository.GetByIdAsync(request.Id);
            if (apppointment == null)
                throw new NotFoundException("该预约不存在");
            var isScheduled = await _repository.AppointmentIsScheduled(request.Id);
            if (!isScheduled)
            {
                throw new BussinessRuleException("该订单已经不在预约状态，无法更改");
            }
            try
            {
                apppointment.UpdateDentistId(request.DentistId);
                apppointment.UpdateDentalOfficeId(request.DentalOfficeId);
                apppointment.UpdateTime(new TimeInterval(request.StartDate, request.EndDate));
                apppointment.UpdateStatus(request.Status);
                await _unitOfWork.BeginTransactionAsync();
                await _repository.UpdateAsync(apppointment);
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
