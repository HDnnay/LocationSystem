using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Auth.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public LoginRequestDto Request { get; set; } = new LoginRequestDto();
    }
}