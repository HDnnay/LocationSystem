using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos.Users;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.ValueObjects;
using Mapster;

namespace LocationSystem.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> Handle(UpdateUserCommand command)
        {
            try
            {
                // 开始事务
                await _unitOfWork.BeginTransactionAsync();

                // 获取用户实体
                var user = await _userRepository.GetByIdAsync(command.Id);
                if (user == null)
                {
                    throw new Exception("用户不存在");
                }

                // 更新用户属性
                user.UpdateName(command.Name);
                user.UpdateEmail(new Email(command.Email));

                // 检查用户类型是否需要更新
                //var newUserType = Enum.Parse<UserType>(command.UserType);
                //if (user.UserType != newUserType)
                //{
                //    // 注意：UserType 是实体鉴别器，一旦保存就不能修改
                //    // 这里我们不尝试修改 UserType，只更新其他属性
                //    // 如果需要改变用户类型，需要创建新用户并迁移数据
                //}

                // 更新用户
                await _userRepository.UpdateAsync(user);

                // 提交事务
                await _unitOfWork.CommitAsync();

                return user.Adapt<UserDto>();
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
