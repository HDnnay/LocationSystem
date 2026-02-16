namespace LocationSystem.Application.Features.Auth.Login.Commands
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}