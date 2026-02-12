using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Exceptions;
using LocationSystem.Domain.Entities;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Application.Features.Roles.Commands.DisableRole
{
    public class DisableRoleCommandHandler : IRequestHandler<DisableRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DisableRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool>
        Handle(DisableRoleCommand request)
        {
            try
            {
                // 开始事务
                await _unitOfWork.BeginTransactionAsync();

                var role = await _roleRepository.GetByIdAsync(request.RoleId);
                if (role == null)
                {
                    throw new NotFoundException("角色不存在");
                }

                role.Disable();
                await _roleRepository.UpdateAsync(role);

                // 提交事务
                await _unitOfWork.CommitAsync();

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
