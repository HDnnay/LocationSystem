using LocationSystem.Presentation.InputTypes;
using LocationSystem.Presentation.Models;

namespace LocationSystem.Presentation.GraphQL
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            // 用户相关操作
            descriptor.Field(m => m.CreateUserAsync(default!, default!))
                .Name("createUser")
                .Description("创建用户")
                .Argument("command", a => a.Type<NonNullType<CreateUserCommandType>>())
                .Type<NonNullType<UserType>>();
            descriptor.Field(t => t.LoginAsync(default!, default!)).Name("login").Description("登录")
                .Argument("command", t => t.Type<NonNullType<LoginCommandType>>())
        }
    }
}
