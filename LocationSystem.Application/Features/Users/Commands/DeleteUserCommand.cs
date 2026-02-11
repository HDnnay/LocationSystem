using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }
}
