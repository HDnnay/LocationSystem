using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand>
    {
        private readonly IAppointmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteAppointmentCommandHandler(IAppointmentRepository repository,IUnitOfWork unitOfWork) 
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteAppointmentCommand request)
        {
            var appiontments = await _repository.GetByIdAsync(request.Id);
            if (appiontments == null)
                throw new NotFoundException("删除的预约不存在");
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _repository.DeleteAsync(appiontments);
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
