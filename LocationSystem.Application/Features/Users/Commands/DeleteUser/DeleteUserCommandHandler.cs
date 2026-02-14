using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserCommand command)
        {
            try
            {
                // 开始事务
                await _unitOfWork.BeginTransactionAsync();

                // 获取用户
                var user = await _userRepository.GetByIdAsync(command.UserId);
                if (user == null)
                {
                    throw new NotFoundException("用户不存在");
                }

                // 检查是否是超级管理员
                if (user.IsSuperAdmin)
                {
                    throw new ApplicationCustomException("超级管理员不允许删除", 403);
                }

                // 检查是否是删除自己
                if (command.UserId == command.CurrentUserId)
                {
                    throw new ApplicationCustomException("不能删除自己的账户", 400);
                }

                // 删除用户
                await _userRepository.DeleteAsync(user);

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
