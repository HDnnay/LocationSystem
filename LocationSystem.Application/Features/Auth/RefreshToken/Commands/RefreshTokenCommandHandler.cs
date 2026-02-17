using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Features.Auth.Login.Commands;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Jwt;
using LocationSystem.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace LocationSystem.Application.Features.Auth.RefreshToken.Commands
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(IUserRepository userRepository, IJwtService jwtService,ILogger<RefreshTokenCommandHandler>logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<LoginResponseDto> Handle(RefreshTokenCommand request)
        {
            try
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
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogWarning("令牌验证失败：令牌已过期 - {Message}", ex.Message);
                return null;
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                _logger.LogWarning("令牌验证失败：签名无效 - {Message}", ex.Message);
                return null;
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "令牌验证失败: {Message}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "令牌验证时发生未知错误");
                return null;
            }
        }
    }
}