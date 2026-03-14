using HotChocolate;
using HotChocolate.Types;
using LocationSystem.Application.Features.Menus.Commands.AssignPermissionsToMenu;
using LocationSystem.Application.Features.Menus.Commands.CreateMenu;
using LocationSystem.Application.Features.Menus.Commands.DeleteMenu;
using LocationSystem.Application.Features.Menus.Commands.UpdateMenu;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Utilities;
using System;

namespace LocationSystem.Api.GraphQL
{
    public class SuccessResponseType : ObjectType<SuccessResponse>
    {
        protected override void Configure(IObjectTypeDescriptor<SuccessResponse> descriptor)
        {
            descriptor.Field(r => r.Success).Type<NonNullType<BooleanType>>();
        }
    }

    public class SuccessResponse
    {
        public bool Success { get; set; }
    }

    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(m => m.CreateMenu(default!))
                .Name("createMenu")
                .Description("创建菜单")
                .Argument("command", a => a.Type<NonNullType<CreateMenuCommandType>>())
                .Type<MenuType>();

            descriptor.Field(m => m.UpdateMenu(default!, default!))
                .Name("updateMenu")
                .Description("更新菜单")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("command", a => a.Type<NonNullType<UpdateMenuCommandType>>())
                .Type<MenuType>();

            descriptor.Field(m => m.DeleteMenu(default!))
                .Name("deleteMenu")
                .Description("删除菜单")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Type<SuccessResponseType>();

            descriptor.Field(m => m.AssignPermissionsToMenu(default!, default!))
                .Name("assignPermissionsToMenu")
                .Description("为菜单分配权限")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("permissionIds", a => a.Type<NonNullType<ListType<NonNullType<IdType>>>>())
                .Type<SuccessResponseType>();
        }
    }

    public class CreateMenuCommandType : InputObjectType<CreateMenuCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateMenuCommand> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Path).Type<StringType>();
            descriptor.Field(c => c.Icon).Type<StringType>();
            descriptor.Field(c => c.Order).Type<IntType>();
            descriptor.Field(c => c.ParentId).Type<IdType>();
        }
    }

    public class UpdateMenuCommandType : InputObjectType<UpdateMenuCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateMenuCommand> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Path).Type<StringType>();
            descriptor.Field(c => c.Icon).Type<StringType>();
            descriptor.Field(c => c.Order).Type<IntType>();
            descriptor.Field(c => c.ParentId).Type<IdType>();
        }
    }

    public class Mutation
    {
        private readonly IMediator _mediator;

        public Mutation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<MenuDto> CreateMenu(CreateMenuCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<MenuDto> UpdateMenu(Guid id, UpdateMenuCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        public async Task<SuccessResponse> DeleteMenu(Guid id)
        {
            var command = new DeleteMenuCommand { MenuId = id };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        public async Task<SuccessResponse> AssignPermissionsToMenu(Guid id, List<Guid> permissionIds)
        {
            var command = new AssignPermissionsToMenuCommand { MenuId = id, PermissionIds = permissionIds };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }
    }
}