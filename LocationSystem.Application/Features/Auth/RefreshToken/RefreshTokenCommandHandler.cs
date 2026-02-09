using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Jwt;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> Handle(RefreshTokenCommand request)
        {
            var refreshTokenRequest = request.Request;
            var refreshToken = refreshTokenRequest.RefreshToken;

            // 查找用户（实际项目中应该根据refresh token从数据库查询用户）
            // 这里简化处理，根据用户类型和已知信息查找用户
            User? user = null;
            if (refreshTokenRequest.Type == UserType.Dentist)
            {
                // 实际项目中应该根据refresh token从数据库查询用户
                // 这里简化处理，使用现有的方法
                user = await _userRepository.GetDentistByEmail("dentist@example.com");
            }
            else if (refreshTokenRequest.Type == UserType.Patient)
            {
                // 实际项目中应该根据refresh token从数据库查询用户
                // 这里简化处理，使用现有的方法
                user = await _userRepository.GetPatientByEmail("patient@example.com");
            }

            if (user == null)
            {
                throw new Exception("用户不存在");
            }

            // 验证refresh token
            var storedRefreshToken = await _userRepository.GetRefreshToken(user.Id);
            if (storedRefreshToken != refreshToken)
            {
                throw new Exception("无效的刷新令牌");
            }

            // 生成新的token
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // 更新refresh token
            await _userRepository.SaveRefreshToken(user.Id, newRefreshToken);

            // 构建响应
            var response = new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
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
}