using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
}
}