using LocationSystem.Application.GrapqLDTOs;

namespace LocationSystem.Presentation.Models
{
    public class UserType : ObjectType<UserGrapqLDto>  // ← 直接使用 Application 层的 DTO
    {
        protected override void Configure(IObjectTypeDescriptor<UserGrapqLDto> descriptor)
        {
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>().Description("用户ID");
            descriptor.Field(t => t.Name).Type<StringType>().Description("用户名");
            descriptor.Field(t => t.Email).Type<StringType>().Description("邮箱");
        }
    }
}