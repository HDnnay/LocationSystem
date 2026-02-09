using LocationSystem.Application.Features.Auth.Login;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenCommand : IRequset<LoginResponseDto>
    {
        public RefreshTokenRequestDto Request { get; set; } = new RefreshTokenRequestDto();
    }
}