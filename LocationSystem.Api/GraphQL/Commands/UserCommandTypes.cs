using HotChocolate.Types;
using LocationSystem.Application.Features.Users.Commands.CreateUser;
using LocationSystem.Application.Features.Users.Commands.UpdateUser;

namespace LocationSystem.Api.GraphQL.Commands
{
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
}