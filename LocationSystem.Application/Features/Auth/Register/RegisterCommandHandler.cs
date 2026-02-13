using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;

namespace LocationSystem.Application.Features.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
    {
        private readonly LocationSystem.Application.Contrats.Repositories.IUserRepository _userRepository;
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
            var existingUser = await _userRepository.GetUserByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                throw new Exception("邮箱已被注册");
            }

            // 根据用户类型创建用户
            var emailValue = new Email(registerRequest.Email);
            var user = new DefaultUser(registerRequest.Name, emailValue, registerRequest.Type);

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
