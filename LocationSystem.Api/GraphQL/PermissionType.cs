using HotChocolate.Types;
using LocationSystem.Application.Dtos;

namespace LocationSystem.Api.GraphQL
{
    public class PermissionType : ObjectType<PermissionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PermissionDto> descriptor)
        {
            descriptor.Field(p => p.Id).Type<NonNullType<IdType>>();
            descriptor.Field(p => p.Name).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.Code).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.Description).Type<StringType>();
            descriptor.Field(p => p.CreatedAt).Type<NonNullType<DateTimeType>>();
            descriptor.Field(p => p.UpdatedAt).Type<DateTimeType>();
            descriptor.Field(p => p.ParentId).Type<IdType>();
            descriptor.Field(p => p.ChildPermissions).Type<ListType<PermissionType>>();
        }
    }
}