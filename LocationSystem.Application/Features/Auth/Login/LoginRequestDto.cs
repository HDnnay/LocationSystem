using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Login
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserType Type { get; set; }
    }
}