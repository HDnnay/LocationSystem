using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;

namespace LocationSystem.Application.Features.Auth.Register
{
    public class RegisterCommandHandler : IRequsetHandler<RegisterCommand, RegisterResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserRegistrationStrategyFactory _strategyFactory;

        public RegisterCommandHandler(IUserRepository userRepository, UserRegistrationStrategyFactory strategyFactory)
        {
            _userRepository = userRepository;
            _strategyFactory = strategyFactory;
        }

        public async Task<RegisterResponseDto> Handle(RegisterCommand request)
        {
            var registerRequest = request.Request;

            // 验证邮箱是否已存在
            var isEmailExists = await _userRepository.IsEmailExists(registerRequest.Email);
            if (isEmailExists)
            {
                throw new Exception("邮箱已被注册");
            }

            // 根据用户类型创建用户
            User user;
            if (registerRequest.Type == UserType.Dentist)
            {
                user = new Dentist(registerRequest.Name, new Email(registerRequest.Email));
            }
            else if (registerRequest.Type == UserType.Patient)
            {
                user = new Patient(registerRequest.Name, new Email(registerRequest.Email));
            }
            else
            {
                throw new Exception("无效的用户类型");
            }

            // 设置密码哈希
            user.SetPasswordHash(registerRequest.Password);

            // 使用策略注册用户
            var strategy = _strategyFactory.GetStrategy(registerRequest.Type);
            await strategy.RegisterUser(user, _userRepository);

            return new RegisterResponseDto
            {
                Message = "注册成功",
                Success = true
            };
        }
    }
}
