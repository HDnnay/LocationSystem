using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Events;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Users.Commands.AssignRoles
{
    public class AssignRolesCommandHandler : IRequestHandler<AssignRolesCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;

        public AssignRolesCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork, IEventBus eventBus)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }

        public async Task<bool> Handle(AssignRolesCommand command)
        {
            try
            {
                // 开始事务
                await _unitOfWork.BeginTransactionAsync();

                // 获取用户
                var user = await _userRepository.GetByIdAsync(command.UserId);
                if (user == null)
                {
                    throw new Exception("用户不存在");
                }

                // 清除用户现有角色
                user.Roles.Clear();

                // 添加新角色
                if (command.RoleIds != null && command.RoleIds.Count > 0)
                {
                    foreach (var roleId in command.RoleIds)
                    {
                        var role = await _roleRepository.GetByIdAsync(roleId);
                        if (role != null)
                        {
                            user.Roles.Add(role);
                        }
                    }
                }

                // 保存更改
                await _userRepository.UpdateAsync(user);

                // 提交事务
                await _unitOfWork.CommitAsync();

                // 发布用户角色变更事件，更新缓存
                await _eventBus.PublishAsync(new UserRolesChangedEvent { UserId = user.Id });

                return true;
            }
            catch (Exception ex)
            {
                // 回滚事务
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
