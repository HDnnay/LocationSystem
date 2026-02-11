using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Jwt;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
    {
        private readonly LocationSystem.Application.Contrats.Repositories.IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(LocationSystem.Application.Contrats.Repositories.IUserRepository userRepository, IJwtService jwtService)
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
            // 实际项目中应该根据refresh token从数据库查询用户
            // 这里简化处理，使用现有的方法
            user = await _userRepository.GetUserByEmailAsync("dentist@example.com");

            if (user == null)
            {
                throw new Exception("用户不存在");
            }

            // 生成新的token
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

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