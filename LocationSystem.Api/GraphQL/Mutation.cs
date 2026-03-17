using HotChocolate;
using HotChocolate.Types;
using Dtos = LocationSystem.Application.Dtos;
using MenuModels = LocationSystem.Application.Features.Menus.Models;
using UserModels = LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Features.Menus.Commands.AssignPermissionsToMenu;
using LocationSystem.Application.Features.Menus.Commands.CreateMenu;
using LocationSystem.Application.Features.Menus.Commands.DeleteMenu;
using LocationSystem.Application.Features.Menus.Commands.UpdateMenu;
using LocationSystem.Application.Features.Users.Commands.AssignRoles;
using LocationSystem.Application.Features.Users.Commands.CreateUser;
using LocationSystem.Application.Features.Users.Commands.DeleteUser;
using LocationSystem.Application.Features.Users.Commands.UpdateUser;
using LocationSystem.Application.Features.Roles.Commands.CreateRole;
using LocationSystem.Application.Features.Roles.Commands.DeleteRole;
using LocationSystem.Application.Features.Roles.Commands.UpdateRole;
using LocationSystem.Application.Utilities;
using System;
using System.Collections.Generic;

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
            // 菜单相关操作
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

            // 用户相关操作
            descriptor.Field(m => m.CreateUser(default!))
                .Name("createUser")
                .Description("创建用户")
                .Argument("command", a => a.Type<NonNullType<CreateUserCommandType>>())
                .Type<NonNullType<IdType>>();

            descriptor.Field(m => m.UpdateUser(default!, default!))
                .Name("updateUser")
                .Description("更新用户")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("command", a => a.Type<NonNullType<UpdateUserCommandType>>())
                .Type<UserType>();

            descriptor.Field(m => m.DeleteUser(default!))
                .Name("deleteUser")
                .Description("删除用户")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Type<SuccessResponseType>();

            descriptor.Field(m => m.AssignRolesToUser(default!, default!))
                .Name("assignRolesToUser")
                .Description("为用户分配角色")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("roleIds", a => a.Type<NonNullType<ListType<NonNullType<IdType>>>>())
                .Type<SuccessResponseType>();

            // 角色相关操作
            descriptor.Field(m => m.CreateRole(default!))
                .Name("createRole")
                .Description("创建角色")
                .Argument("command", a => a.Type<NonNullType<CreateRoleCommandType>>())
                .Type<RoleType>();

            descriptor.Field(m => m.UpdateRole(default!, default!))
                .Name("updateRole")
                .Description("更新角色")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Argument("command", a => a.Type<NonNullType<UpdateRoleCommandType>>())
                .Type<RoleType>();

            descriptor.Field(m => m.DeleteRole(default!))
                .Name("deleteRole")
                .Description("删除角色")
                .Argument("id", a => a.Type<NonNullType<IdType>>())
                .Type<SuccessResponseType>();

            descriptor.Field(m => m.AssignPermissionsToRole(default!, default!))
                .Name("assignPermissionsToRole")
                .Description("为角色分配权限")
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

    // 用户相关命令类型
    public class CreateUserCommandType : InputObjectType<CreateUserCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateUserCommand> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Email).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.UserType).Type<NonNullType<StringType>>();
        }
    }

    public class UpdateUserCommandType : InputObjectType<UpdateUserCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateUserCommand> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Email).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.UserType).Type<NonNullType<StringType>>();
        }
    }

    // 角色相关命令类型
    public class CreateRoleCommandType : InputObjectType<CreateRoleCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateRoleCommand> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Code).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Description).Type<StringType>();
            descriptor.Field(c => c.PermissionIds).Type<ListType<IdType>>();
        }
    }

    public class UpdateRoleCommandType : InputObjectType<UpdateRoleCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateRoleCommand> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Code).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Description).Type<StringType>();
            descriptor.Field(c => c.PermissionIds).Type<ListType<IdType>>();
        }
    }

    public class Mutation
    {
        private readonly IMediator _mediator;

        public Mutation(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 菜单相关操作
        public async Task<MenuModels.MenuDto> CreateMenu(CreateMenuCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<MenuModels.MenuDto> UpdateMenu(Guid id, UpdateMenuCommand command)
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

        // 用户相关操作
        public async Task<Guid> CreateUser(CreateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<UserModels.UserDto> UpdateUser(Guid id, UpdateUserCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        public async Task<SuccessResponse> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand { UserId = id };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        public async Task<SuccessResponse> AssignRolesToUser(Guid id, List<Guid> roleIds)
        {
            var command = new AssignRolesCommand { UserId = id, RoleIds = roleIds };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        // 角色相关操作
        public async Task<Dtos.RoleDto> CreateRole(CreateRoleCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<Dtos.RoleDto> UpdateRole(Guid id, UpdateRoleCommand command)
        {
            command.RoleId = id;
            return await _mediator.Send(command);
        }

        public async Task<SuccessResponse> DeleteRole(Guid id)
        {
            var command = new DeleteRoleCommand { RoleId = id };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }

        public async Task<SuccessResponse> AssignPermissionsToRole(Guid id, List<Guid> permissionIds)
        {
            var command = new UpdateRoleCommand { RoleId = id, PermissionIds = permissionIds };
            await _mediator.Send(command);
            return new SuccessResponse { Success = true };
        }
    }
}