using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Features.Roles.Commands.DeleteRole;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Features.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequsetHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteRoleCommand request)
        {
            // 检查角色是否存在
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new Exception($"角色不存在，ID: {request.RoleId}");
            }

            // 删除角色
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _roleRepository.DeleteAsync(role);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            return true;
        }
    }
}