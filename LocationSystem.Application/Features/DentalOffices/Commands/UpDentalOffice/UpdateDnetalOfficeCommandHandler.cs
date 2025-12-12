using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.DentalOffices.Commands.UpDentalOffice
{
    public class UpdateDnetalOfficeCommandHandler : IRequestHandler<UpdateDetalOfficeCommand>
    {
        private readonly IDentalOfficeRepositoty _repositoty;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateDnetalOfficeCommandHandler(IDentalOfficeRepositoty repositoty,IUnitOfWork unitOfWork) 
        {
            _repositoty = repositoty;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateDetalOfficeCommand request)
        {
            var dentalOffice = await _repositoty.GetByIdAsync(request.Id);
            if (dentalOffice == null) 
            {
                throw new Exception($"{nameof(dentalOffice)}为空");
            }
            dentalOffice.UpdateName(request.Name);
            try
            {
                await _repositoty.UpdateAsync(dentalOffice);
                await _unitOfWork.Commit();
                
            }
            catch (Exception ) {
                await _unitOfWork.Rollback();
            }
        }
    }
}
