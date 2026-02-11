using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly LocationSystem.Application.Contrats.Repositories.IUserRepository _userRepository;

        public DeleteUserCommandHandler(LocationSystem.Application.Contrats.Repositories.IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand command)
        {
            // 获取用户
            var user = await _userRepository.GetByIdAsync(command.UserId);
            if (user == null)
            {
                throw new Exception("用户不存在");
            }

            // 删除用户
            await _userRepository.DeleteAsync(user);
            return true;
        }
    }
}
