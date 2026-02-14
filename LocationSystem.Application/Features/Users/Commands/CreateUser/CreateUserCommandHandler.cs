using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.Factories;

namespace LocationSystem.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserFactory _userFactory;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userFactory = new UserFactory();
        }

        public async Task<Guid> Handle(CreateUserCommand command)
        {
            try
            {
                var userExs=await _userRepository.GetUserByEmailAsync(command.Email);
                if (userExs!=null)
                    throw new ArgumentException("该邮箱已被注册");
                // 开始事务
                await _unitOfWork.BeginTransactionAsync();

                // 创建用户实体
                var userType = command.UserType;
                // 使用默认密码 "123456"
                var user = _userFactory.CreateUser(userType, command.Name, command.Email, "123456",false);

                // 添加用户
                await _userRepository.AddAsync(user);

                // 提交事务
                await _unitOfWork.CommitAsync();

                return user.Id;
            }
            catch (Exception ex)
            {
                // 回滚事务
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
