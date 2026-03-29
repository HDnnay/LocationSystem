using HotChocolate.Types;
using LocationSystem.Api.GraphQL.Commands;
using LocationSystem.Api.GraphQL.Mutations;
using LocationSystem.Api.GraphQL.Types;
using LocationSystem.Application.Features.Menus.Commands.AssignPermissionsToMenu;
using LocationSystem.Application.Features.Menus.Commands.CreateMenu;
using LocationSystem.Application.Features.Menus.Commands.DeleteMenu;
using LocationSystem.Application.Features.Menus.Commands.UpdateMenu;
using LocationSystem.Application.Features.Menus.Models;
using LocationSystem.Application.Features.Roles.Commands.CreateRole;
using LocationSystem.Application.Features.Roles.Commands.DeleteRole;
using LocationSystem.Application.Features.Roles.Commands.UpdateRole;
using LocationSystem.Application.Features.Users.Commands.AssignRoles;
using LocationSystem.Application.Features.Users.Commands.CreateUser;
using LocationSystem.Application.Features.Users.Commands.DeleteUser;
using LocationSystem.Application.Features.Users.Commands.UpdateUser;
using LocationSystem.Application.Features.Users.Models;
using LocationSystem.Application.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dtos = LocationSystem.Application.Dtos;

namespace LocationSystem.Api.GraphQL
{
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

    public class Mutation
    {
        private readonly MenuMutation _menuMutation;
        private readonly UserMutation _userMutation;
        private readonly RoleMutation _roleMutation;

        public Mutation(IMediator mediator)
        {
            _menuMutation = new MenuMutation(mediator);
            _userMutation = new UserMutation(mediator);
            _roleMutation = new RoleMutation(mediator);
        }

        // 菜单相关操作
        public async Task<MenuDto> CreateMenu(CreateMenuCommand command)
        {
            return await _menuMutation.CreateMenu(command);
        }

        public async Task<MenuDto> UpdateMenu(Guid id, UpdateMenuCommand command)
        {
            return await _menuMutation.UpdateMenu(id, command);
        }

        public async Task<SuccessResponse> DeleteMenu(Guid id)
        {
            return await _menuMutation.DeleteMenu(id);
        }

        public async Task<SuccessResponse> AssignPermissionsToMenu(Guid id, List<Guid> permissionIds)
        {
            return await _menuMutation.AssignPermissionsToMenu(id, permissionIds);
        }

        // 用户相关操作
        public async Task<Guid> CreateUser(CreateUserCommand command)
        {
            return await _userMutation.CreateUser(command);
        }

        public async Task<UserDto> UpdateUser(Guid id, UpdateUserCommand command)
        {
            return await _userMutation.UpdateUser(id, command);
        }

        public async Task<SuccessResponse> DeleteUser(Guid id)
        {
            return await _userMutation.DeleteUser(id);
        }

        public async Task<SuccessResponse> AssignRolesToUser(Guid id, List<Guid> roleIds)
        {
            return await _userMutation.AssignRolesToUser(id, roleIds);
        }

        // 角色相关操作
        public async Task<Dtos.RoleDto> CreateRole(CreateRoleCommand command)
        {
            return await _roleMutation.CreateRole(command);
        }

        public async Task<Dtos.RoleDto> UpdateRole(Guid id, UpdateRoleCommand command)
        {
            return await _roleMutation.UpdateRole(id, command);
        }

        public async Task<SuccessResponse> DeleteRole(Guid id)
        {
            return await _roleMutation.DeleteRole(id);
        }

        public async Task<SuccessResponse> AssignPermissionsToRole(Guid id, List<Guid> permissionIds)
        {
            return await _roleMutation.AssignPermissionsToRole(id, permissionIds);
        }
    }


}