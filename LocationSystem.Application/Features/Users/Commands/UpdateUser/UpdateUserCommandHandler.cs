using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;

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
                user.UpdataUserType(Enum.Parse<UserType>(command.UserType));

                // 更新用户
                await _userRepository.UpdateAsync(user);

                // 提交事务
                await _unitOfWork.CommitAsync();

                return new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email.Value,
                    UserType = user.UserType.ToString(),
                    Roles = user.Roles.Select(role => new RoleDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Code = role.Code
                    }).ToList()
                };
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
