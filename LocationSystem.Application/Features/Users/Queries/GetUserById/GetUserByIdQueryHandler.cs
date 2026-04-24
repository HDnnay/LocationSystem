using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Dtos.Roles;
using LocationSystem.Application.Dtos.Users;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.Users.Queries
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
                IsDisabled = user.IsDisabled,
                Roles = user.Roles.Select(role => role.Adapt<RoleDto>()).ToList()
            };
        }
    }
}
