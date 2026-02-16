using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;

namespace LocationSystem.Application.Features.Auth.Register.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
    {
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            var user = new RegularUser(registerRequest.Name, emailValue,UserType.User);

            // 设置密码哈希
            user.SetPasswordHash(registerRequest.Password);

            // 直接保存用户
            await _userRepository.AddAsync(user);

            return new RegisterResponseDto
            {
                Message = "注册成功",
                Success = true
            };
        }
    }
}
