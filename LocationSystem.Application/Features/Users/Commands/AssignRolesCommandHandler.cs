using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands
{
    public class AssignRolesCommandHandler : IRequestHandler<AssignRolesCommand, bool>
    {
        private readonly LocationSystem.Application.Contrats.Repositories.IUserRepository _userRepository;

        public AssignRolesCommandHandler(LocationSystem.Application.Contrats.Repositories.IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(AssignRolesCommand command)
        {
            // 获取用户
            var user = await _userRepository.GetByIdAsync(command.UserId);
            if (user == null)
            {
                throw new Exception("用户不存在");
            }

            // 清除用户现有角色
            user.Roles.Clear();

            // 保存更改
            await _userRepository.UpdateAsync(user);

            return true;
        }
    }
}
