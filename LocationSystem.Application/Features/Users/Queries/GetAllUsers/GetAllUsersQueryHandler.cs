using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using LocationSystem.Domain.Entities;

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

            pageResult.Data= users.Item2.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email.Value,
                UserType = user.UserType.ToString(),
                Roles = user.Roles.Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Code = role.Code
                }).ToList()
            }).ToList();
            return pageResult;
        }
    }
}
