using LocationSystem.Api.GraphQL.Types;
using LocationSystem.Application.Features.Users.Commands.AssignRoles;
using LocationSystem.Application.Features.Users.Commands.CreateUser;
using LocationSystem.Application.Features.Users.Commands.DeleteUser;
using LocationSystem.Application.Features.Users.Commands.UpdateUser;
using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationSystem.Api.GraphQL.Mutations
{
    public class UserMutation
    {
        private readonly IMediator _mediator;

        public UserMutation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<System.Guid> CreateUser(CreateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<UserDto> UpdateUser(System.Guid id, UpdateUserCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        public async Task<SuccessResponse> DeleteUser(System.Guid id)
        {
            var command = new DeleteUserCommand { UserId = id };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        public async Task<SuccessResponse> AssignRolesToUser(System.Guid id, List<System.Guid> roleIds)
        {
            var command = new AssignRolesCommand { UserId = id, RoleIds = roleIds };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }
    }
}