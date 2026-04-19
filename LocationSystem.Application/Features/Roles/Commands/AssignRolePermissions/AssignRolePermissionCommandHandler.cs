using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Dtos.Roles;
using LocationSystem.Application.Events;
using LocationSystem.Application.Utilities;
using Mapster;

namespace LocationSystem.Application.Features.Roles.Commands.AssignRolePermissions
{
    public class AssignRolePermissionCommandHandler : IRequestHandler<AssignRolePermissionCommand, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;

        public AssignRolePermissionCommandHandler(
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IUnitOfWork unitOfWork,
            IEventBus eventBus)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }

        public async Task<RoleDto> Handle(AssignRolePermissionCommand request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var role = await _roleRepository.GetRoleWithPermissionsAsync(request.RoleId);
                if (role == null)
                {
                    throw new Exception("角色不存在");
                }

                var currentPermissionIds = role.Permissions.Select(p => p.Id).ToHashSet();
                var newPermissionIds = request.Permissions?.ToHashSet() ?? new HashSet<Guid>();

                var toRemove = role.Permissions
                    .Where(p => !newPermissionIds.Contains(p.Id))
                    .ToList();

                foreach (var permission in toRemove)
                {
                    role.RemovePermission(permission);
                }

                var toAddIds = newPermissionIds.Except(currentPermissionIds);
                foreach (var permissionId in toAddIds)
                {
                    var permission = await _permissionRepository.GetByIdAsync(permissionId);
                    if (permission != null)
                    {
                        role.AddPermission(permission);
                    }
                }

                await _roleRepository.UpdateAsync(role);

                await _unitOfWork.CommitAsync();

                await _eventBus.PublishAsync(new RolePermissionsChangedEvent { RoleId = role.Id });

                var roleDto = role.Adapt<RoleDto>();
                return roleDto;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
