using LocationSystem.Api.GraphQL.Types;
using LocationSystem.Application.Dtos;
using LocationSystem.Application.Features.Roles.Commands.CreateRole;
using LocationSystem.Application.Features.Roles.Commands.DeleteRole;
using LocationSystem.Application.Features.Roles.Commands.UpdateRole;
using LocationSystem.Application.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationSystem.Api.GraphQL.Mutations
{
    public class RoleMutation
    {
        private readonly IMediator _mediator;

        public RoleMutation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RoleDto> CreateRole(CreateRoleCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<RoleDto> UpdateRole(System.Guid id, UpdateRoleCommand command)
        {
            command.RoleId = id;
            return await _mediator.Send(command);
        }

        public async Task<SuccessResponse> DeleteRole(System.Guid id)
        {
            var command = new DeleteRoleCommand { RoleId = id };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        public async Task<SuccessResponse> AssignPermissionsToRole(System.Guid id, List<System.Guid> permissionIds)
        {
            var command = new UpdateRoleCommand { RoleId = id, PermissionIds = permissionIds };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }
    }
}