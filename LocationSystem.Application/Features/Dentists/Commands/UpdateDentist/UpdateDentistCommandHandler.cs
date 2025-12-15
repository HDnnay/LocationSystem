using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Features.Dentists.Commands.UpdateDentist
{
    public class UpdateDentistCommandHandler : IRequestHandler<UpdateDentistCommand>
    {
        private readonly IDentistRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDentistCommandHandler(IDentistRepository repository,IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateDentistCommand request)
        {
            var dentits = await _repository.GetByIdAsync(request.Id);
            if (dentits is null)
                throw new NotFoundException("编辑的detits不存在");
            dentits.UpdateName(request.Name);
            dentits.UpdateEmail(new Email(request.Email));
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _repository.UpdateAsync(dentits);
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
