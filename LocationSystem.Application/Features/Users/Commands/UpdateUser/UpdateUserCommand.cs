using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
    }
}
