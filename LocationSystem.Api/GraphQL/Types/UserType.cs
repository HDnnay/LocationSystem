using HotChocolate.Types;
using LocationSystem.Application.Features.Users.Models;

namespace LocationSystem.Api.GraphQL.Types
{
    public class UserType : ObjectType<UserDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserDto> descriptor)
        {
            descriptor.Field(u => u.Id).Type<NonNullType<IdType>>();
            descriptor.Field(u => u.Name).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.Email).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.UserType).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.IsDisabled).Type<NonNullType<BooleanType>>();
            descriptor.Field(u => u.Roles).Type<ListType<RoleType>>();
        }
    }
}