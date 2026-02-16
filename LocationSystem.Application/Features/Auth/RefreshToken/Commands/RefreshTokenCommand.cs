using LocationSystem.Application.Features.Auth.Login.Commands;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Auth.RefreshToken.Commands
{
    public class RefreshTokenCommand : IRequest<LoginResponseDto>
    {
        public RefreshTokenRequestDto Request { get; set; } = new RefreshTokenRequestDto();
    }
}