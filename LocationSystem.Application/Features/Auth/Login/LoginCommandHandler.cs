using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Jwt;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Login
{
    public class LoginCommandHandler : IRequsetHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request)
        {
            var loginRequest = request.Request;
            
            // 根据用户类型查询用户
            User? user = null;
            if (loginRequest.Type == UserType.Dentist)
            {
                user = await _userRepository.GetDentistByEmail(loginRequest.Email);
            }
            else if (loginRequest.Type == UserType.Patient)
            {
                user = await _userRepository.GetPatientByEmail(loginRequest.Email);
            }
            if (user == null)
            {
                throw new Exception("用户不存在");
            }

            // 验证密码
            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                throw new Exception("密码错误");
            }

            // 生成token
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // 保存refresh token到数据库
            await _userRepository.SaveRefreshToken(user.Id, refreshToken);

            // 构建响应
            var response = new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserInfo = new UserInfoDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email.Value,
                    UserType = user.UserType.ToString()
                }
            };

            return response;
        }
    }

    // 用户仓库接口
    public interface IUserRepository
    {
        Task<Dentist?> GetDentistByEmail(string email);
        Task<Patient?> GetPatientByEmail(string email);
        Task<User?> GetUserByEmail(string email);
        Task<bool> IsEmailExists(string email);
        Task SaveRefreshToken(Guid userId, string refreshToken);
        Task<string?> GetRefreshToken(Guid userId);
        Task AddAsync(User user);
        Task AddDentistAsync(Dentist dentist);
        Task AddPatientAsync(Patient patient);
    }
}