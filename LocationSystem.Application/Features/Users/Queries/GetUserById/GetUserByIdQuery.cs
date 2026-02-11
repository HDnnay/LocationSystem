using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public Guid UserId { get; set; }
    }
}
