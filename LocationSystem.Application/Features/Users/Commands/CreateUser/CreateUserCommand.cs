using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
    }
}
