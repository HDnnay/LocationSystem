using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using Microsoft.Extensions.Logging;

namespace LocationSystem.Application.Features.RentHousies.Command.DelelteRentHose
{
    public class DeleteRentHoseCommandHandle : IRequestHandler<DeleteRentHoseCommand>
    {
        private readonly IRentHouseRepository _houseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteRentHoseCommandHandle> _logger;

        public DeleteRentHoseCommandHandle(IRentHouseRepository houseRepository, IUnitOfWork unitOfWork, ILogger<DeleteRentHoseCommandHandle> logger)
        {
            _houseRepository = houseRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task Handle(DeleteRentHoseCommand request)
        {
            if (request.Id.HasValue)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();
                    var rentHose = await _houseRepository.GetByIdAsync(request.Id.Value);
                    if (rentHose==null)
                        throw new NotFoundException("未发现租房记录");
                    await _houseRepository.DeleteAsync(rentHose);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    await _unitOfWork.RollbackAsync();
                }

            }
        }
    }
}
