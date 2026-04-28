using LocationSystem.Application.GrapqLDTOs.Auth;

namespace LocationSystem.Presentation.Models
{
    public class LoginResponseType : ObjectType<LoginResponseGraphqLDto>
    {
        protected override void Configure(IObjectTypeDescriptor<LoginResponseGraphqLDto> descriptor)
        {
            descriptor.Field(t => t.AccessToken).Type<NonNullType<StringType>>().Description("AccessToken");
            descriptor.Field(t => t.RefreshToken).Type<NonNullType<StringType>>().Description("RefreshToken");
        }
    }
}
