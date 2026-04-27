using HotChocolate.Types;
using LocationSystem.Application.Features.Auth.Login.Commands;

namespace LocationSystem.Presentation.InputTypes
{
    public class LoginCommandType : InputObjectType<LoginCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<LoginCommand> descriptor)
        {
            descriptor.Name("LoginCommand");
            
            // 配置 Request 字段
            descriptor.Field(c => c.Request)
                .Type<NonNullType<LoginRequestType>>()
                .Description("登录请求参数");
        }
    }

    public class LoginRequestType : InputObjectType<LoginRequestDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<LoginRequestDto> descriptor)
        {
            descriptor.Name("LoginRequest");
            
            // 配置邮箱字段
            descriptor.Field(r => r.Email)
                .Type<NonNullType<StringType>>()
                .Description("用户邮箱");
            
            // 配置密码字段
            descriptor.Field(r => r.Password)
                .Type<NonNullType<StringType>>()
                .Description("用户密码");
        }
    }
}