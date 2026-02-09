using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.Register
{
    public class RegisterRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
    }
}
