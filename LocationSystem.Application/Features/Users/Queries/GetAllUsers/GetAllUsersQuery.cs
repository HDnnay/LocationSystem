using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;

namespace LocationSystem.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<PageResult<UserDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? keyWord { get; set; } = string.Empty;
    }
}
