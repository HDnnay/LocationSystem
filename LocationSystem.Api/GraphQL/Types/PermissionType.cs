using HotChocolate.Types;
using LocationSystem.Application.Dtos;

namespace LocationSystem.Api.GraphQL.Types
{
    public class PermissionType : ObjectType<PermissionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PermissionDto> descriptor)
        {
            descriptor.Field(p => p.Id).Type<NonNullType<IdType>>();
            descriptor.Field(p => p.Name).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.Code).Type<NonNullType<StringType>>();
            descriptor.Field(p => p.ParentId).Type<IdType>();
            descriptor.Field(p => p.ChildPermissions).Type<ListType<PermissionType>>();
        }
    }
}