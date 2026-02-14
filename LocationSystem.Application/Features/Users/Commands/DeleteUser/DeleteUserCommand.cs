using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}

