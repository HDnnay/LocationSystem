using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands.EnableUser
{
    public class EnableUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }
}
