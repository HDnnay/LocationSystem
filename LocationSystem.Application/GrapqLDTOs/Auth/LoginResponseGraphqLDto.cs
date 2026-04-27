using LocationSystem.Application.Features.Auth.Login.Commands;
using LocationSystem.Application.GrapqLDTOs.Users;

namespace LocationSystem.Application.GrapqLDTOs.Auth
{
    public class LoginResponseGraphqLDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserGraphqLDto UserInfo { get; set; }
        UserInfoDto
    }
}
