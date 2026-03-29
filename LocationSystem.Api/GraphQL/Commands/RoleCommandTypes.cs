using HotChocolate.Types;
using LocationSystem.Application.Features.Roles.Commands.CreateRole;
using LocationSystem.Application.Features.Roles.Commands.UpdateRole;

namespace LocationSystem.Api.GraphQL.Commands
{
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
}