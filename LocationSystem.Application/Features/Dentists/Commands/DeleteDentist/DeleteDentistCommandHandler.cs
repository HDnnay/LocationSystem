using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using System;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Dentists.Commands.DeleteDentist
{
    public class DeleteDentistCommandHandler : IRequestHandler<DeleteDentistCommand>
    {
        private readonly IDentistRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDentistCommandHandler(IDentistRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteDentistCommand request)
        {
            var dentist = await _repository.GetByIdAsync(request.Id);
            if (dentist is null)
                throw new NotFoundException("删除的牙医不存在");

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _repository.DeleteAsync(dentist);
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