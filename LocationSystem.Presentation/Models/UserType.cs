using LocationSystem.Application.GrapqLDTOs;

namespace LocationSystem.Presentation.Models
{
    public class UserType : ObjectType<UserGrapqLDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserGrapqLDto> descriptor)
        {
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>();
        }
    }

}
