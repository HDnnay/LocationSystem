using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Features.Auth.Login.Commands;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Jwt;
using LocationSystem.Domain.Entities;
using System.Security.Claims;

namespace LocationSystem.Application.Features.Auth.RefreshToken.Commands
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
            var principal = _jwtService.ValidateToken(refreshToken);
            if (principal == null)
            {
                throw new InvalidRefreshTokenException();
            }
            var tokenType = principal.FindFirst("token_type")?.Value;
            if (tokenType != "refresh_token")
            {
                throw new RefreshTokenException("令牌类型错误");
            }
            // 获取用户ID
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                throw new InvalidRefreshTokenException();
            }

            // 根据用户ID获取用户信息
            User? user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidRefreshTokenException();
            }
            // 生成新的 token
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken(user.Id);

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