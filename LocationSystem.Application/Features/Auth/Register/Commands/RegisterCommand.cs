using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Auth.Register.Commands
{
    public class RegisterCommand : IRequest<RegisterResponseDto>
    {
        public RegisterRequestDto Request { get; set; }
    }

    public class RegisterResponseDto
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
