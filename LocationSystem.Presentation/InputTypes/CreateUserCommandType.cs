using LocationSystem.Application.Features.Users.Commands.CreateUser;

namespace LocationSystem.Presentation.InputTypes
{
    public class CreateUserCommandType : InputObjectType<CreateUserCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateUserCommand> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>().Description("名字");
            descriptor.Field(c => c.Email).Type<NonNullType<StringType>>().Description("邮箱");
            descriptor.Field(c => c.UserType).Type<NonNullType<StringType>>().Description("用户类型");
        }
    }
}
