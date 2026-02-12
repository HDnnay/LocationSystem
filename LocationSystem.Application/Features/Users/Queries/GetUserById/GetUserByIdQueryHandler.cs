using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Users.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly LocationSystem.Application.Contrats.Repositories.IUserRepository _userRepository;

        public GetUserByIdQueryHandler(LocationSystem.Application.Contrats.Repositories.IUserRepository userRepository)
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
                Roles = user.Roles.Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Code = role.Code,
                    IsDisabled = role.IsDisabled
                }).ToList()
            };
        }
    }
}
