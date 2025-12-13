using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Commands.DeleteDentalOffice
{
    public class DeleteDentalOfficeCommandHandler : IRequestHandler<DeleteDentalOfficeCommand>
    {
        private readonly IDentalOfficeRepository _repositoty;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteDentalOfficeCommandHandler(IDentalOfficeRepository repositoty,IUnitOfWork unitOfWork) 
        {
            _repositoty = repositoty;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteDentalOfficeCommand request)
        {
            var dentalOffice =await _repositoty.GetByIdAsync(request.Id);
            if (dentalOffice == null)
            {
                throw new ArgumentNullException($"{nameof(dentalOffice)}不存在");
            }
            try
            {
                await _repositoty.DeleteAsync(dentalOffice);
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
