using LocationSystem.Api.GraphQL.Types;
using LocationSystem.Application.Features.Menus.Commands.AssignPermissionsToMenu;
using LocationSystem.Application.Features.Menus.Commands.CreateMenu;
using LocationSystem.Application.Features.Menus.Commands.DeleteMenu;
using LocationSystem.Application.Features.Menus.Commands.UpdateMenu;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationSystem.Api.GraphQL.Mutations
{
    public class MenuMutation
    {
        private readonly IMediator _mediator;

        public MenuMutation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<MenuDto> CreateMenu(CreateMenuCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<MenuDto> UpdateMenu(System.Guid id, UpdateMenuCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        public async Task<SuccessResponse> DeleteMenu(System.Guid id)
        {
            var command = new DeleteMenuCommand { MenuId = id };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        public async Task<SuccessResponse> AssignPermissionsToMenu(System.Guid id, List<System.Guid> permissionIds)
        {
            var command = new AssignPermissionsToMenuCommand { MenuId = id, PermissionIds = permissionIds };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }
    }
}