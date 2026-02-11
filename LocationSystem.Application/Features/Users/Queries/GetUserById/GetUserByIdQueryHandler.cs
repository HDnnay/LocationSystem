using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery query)
        {
            var user = await _userRepository.GetByIdAsync(query.UserId);
            if (user == null)
            {
                return null;
            }

            return new UserDto
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
            };
        }
    }
}
