using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities;

namespace LocationSystem.Application.Features.Users.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly LocationSystem.Application.Contrats.Repositories.IUserRepository _userRepository;

        public UpdateUserCommandHandler(LocationSystem.Application.Contrats.Repositories.IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(UpdateUserCommand command)
        {
            // 更新用户
            await _userRepository.UpdateAsync(command.User);
            return new UserDto
            {
                Id = command.User.Id,
                Name = command.User.Name,
                Email = command.User.Email.Value,
                UserType = command.User.UserType.ToString(),
                Roles = command.User.Roles.Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Code = role.Code
                }).ToList()
            };
        }
    }
}
