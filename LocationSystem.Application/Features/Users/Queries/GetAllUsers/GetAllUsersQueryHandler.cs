using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Users;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using Mapster;

namespace LocationSystem.Application.Features.Users.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PageResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PageResult<UserDto>> Handle(GetAllUsersQuery query)
        {
            var users = await _userRepository.GetUserPage(query);
            var pageResult = new PageResult<UserDto>();
            pageResult.CurrentPage = query.Page;
            pageResult.Total = users.Item1;

            pageResult.Items= users.Item2.Select(user => user.Adapt<UserDto>()).ToList();
            return pageResult;
        }
    }
}
