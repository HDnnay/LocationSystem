using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Events;
using LocationSystem.Application.Exceptions;
using LocationSystem.Application.Utilities;
using LocationSystem.Domain.Entities.UserRolePermissions;

namespace LocationSystem.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;

        public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IEventBus eventBus)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }

        public async Task<bool> Handle(DeleteUserCommand command)
        {
            try
            {
                // 开始事务
                await _unitOfWork.BeginTransactionAsync();

                // 获取用户
                var user = await _userRepository.GetByIdAsync(command.UserId);
                if (user == null)
                {
                    throw new NotFoundException("用户不存在");
                }

                // 检查是否是超级管理员


                // 检查是否是删除自己
                if (command.UserId == command.CurrentUserId)
                {
                    throw new ApplicationCustomException("不能删除自己的账户", 400);
                }

                // 删除用户
                await _userRepository.DeleteUserAsync(user.Id);

                // 提交事务
                await _unitOfWork.CommitAsync();

                // 发布实体删除事件，创建快照
                //await _eventBus.PublishAsync(new EntityDeletedEvent
                //{
                //    EntityType = nameof(User),
                //    EntityId = user.Id,
                //    EntityJson = JsonSerializer.Serialize(user),
                //    DeletedAt = DateTime.UtcNow
                //});
                var deletedUser = string.IsNullOrWhiteSpace(command.DeletedBy) ? "" : command.DeletedBy;
                var deleteReason = string.IsNullOrWhiteSpace(command.DeleteReason) ? "" : command.DeleteReason;           // 或从 command.DeleteReason 获取

                // 使用泛型 EntityDeletedEvent<User> 创建并发布事件
                var deletedEvent = EntityDeletedEvent<User>.Create(
                    user,
                    user.Id,
                    deletedUser,           // 或从 command.CurrentUserName 获取
                    deleteReason        // 或从 command.DeleteReason 获取
                );
                await _eventBus.PublishAsync(deletedEvent);

                return true;
            }
            catch (Exception)
            {
                // 回滚事务
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
