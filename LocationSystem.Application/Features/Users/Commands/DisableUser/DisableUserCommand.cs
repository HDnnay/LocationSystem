using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands.DisableUser
{
    public class DisableUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }
}
