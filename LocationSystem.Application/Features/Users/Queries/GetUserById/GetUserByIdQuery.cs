using LocationSystem.Application.Dtos.Users;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public Guid UserId { get; set; }
    }
}
