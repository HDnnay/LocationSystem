using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Domain.Entities;
using LocationSystem.Domain.ValueObjects;

namespace LocationSystem.Application.Features.Auth.Register
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
            var existingDentist = await _userRepository.GetDentistByEmail(registerRequest.Email);
            var existingPatient = await _userRepository.GetPatientByEmail(registerRequest.Email);

            if (existingDentist != null || existingPatient != null)
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

            // 保存用户到数据库
            await _userRepository.AddAsync(user);
            // 注意：实际项目中可能还需要调用UnitOfWork的SaveChanges方法来提交事务
            // 由于当前的实现中没有UnitOfWork的SaveChanges方法，我们假设通过其他方式提交
            // 实际项目中需要根据具体的实现来调整

            return new RegisterResponseDto
            {
                Message = "注册成功",
                Success = true
            };
        }
    }
}
