using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Patients.Command.DeletePatient
{
    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand>
    {
        private readonly IPatientRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePatientCommandHandler(IPatientRepository repository,IUnitOfWork unitOfWork) 
        {
             _repository= repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeletePatientCommand request)
        {
            var patient = await _repository.GetByIdAsync(request.Id);
            if (patient == null)
                throw new ArgumentNullException("删除的patient不存在");
            try
            {
                await _repository.DeleteAsync(patient);
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
           

        }
    }
}
