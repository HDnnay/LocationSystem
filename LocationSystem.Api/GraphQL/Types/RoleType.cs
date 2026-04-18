using HotChocolate.Types;
using LocationSystem.Application.Dtos.Roles;

namespace LocationSystem.Api.GraphQL.Types
{
    public class RoleType : ObjectType<RoleDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RoleDto> descriptor)
        {
            descriptor.Name("AppRoleDto");
            descriptor.Field(r => r.Id).Type<NonNullType<IdType>>();
            descriptor.Field(r => r.Name).Type<NonNullType<StringType>>();
            descriptor.Field(r => r.Code).Type<NonNullType<StringType>>();
            descriptor.Field(r => r.Description).Type<StringType>();
            descriptor.Field(r => r.IsDisabled).Type<NonNullType<BooleanType>>();
            descriptor.Field(r => r.CreatedAt).Type<NonNullType<DateTimeType>>();
            descriptor.Field(r => r.UpdatedAt).Type<DateTimeType>();
            descriptor.Field(r => r.Permissions).Type<ListType<PermissionType>>();
        }
    }
}