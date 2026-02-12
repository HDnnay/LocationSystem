using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Domain.Entities;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands.EnableUser
{
    public class EnableUserCommandHandler : IRequestHandler<EnableUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnableUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool>
        Handle(EnableUserCommand request)
        {
            try
            {
                // 开始事务
                await _unitOfWork.BeginTransactionAsync();

                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    throw new NotFoundException("用户不存在");
                }

                user.Enable();
                await _userRepository.UpdateAsync(user);

                // 提交事务
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                // 回滚事务
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
