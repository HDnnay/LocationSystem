using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Exceptions;
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

            // 根据 refreshToken 查询用户
            User? user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                throw new InvalidRefreshTokenException();
            }

            // 验证 refreshToken 是否过期
            if (user.RefreshTokenExpiryTime < DateTime.Now)
            {
                throw new RefreshTokenExpiredException();
            }

            // 生成新的 token
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // 保存新的 refreshToken 到数据库
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