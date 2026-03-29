using HotChocolate.Types;
using LocationSystem.Application.Features.Menus.Models;

namespace LocationSystem.Api.GraphQL.Types
{
    public class MenuType : ObjectType<MenuDto>
    {
        protected override void Configure(IObjectTypeDescriptor<MenuDto> descriptor)
        {
            descriptor.Field(m => m.Id).Type<IdType>();
            descriptor.Field(m => m.Name).Type<StringType>();
            descriptor.Field(m => m.Path).Type<StringType>();
            descriptor.Field(m => m.Icon).Type<StringType>();
            descriptor.Field(m => m.Order).Type<IntType>();
            descriptor.Field(m => m.ParentId).Type<IdType>();
            descriptor.Field(m => m.CreatedAt).Type<DateTimeType>();
            descriptor.Field(m => m.UpdatedAt).Type<DateTimeType>();
            descriptor.Field(m => m.ChildMenus).Type<ListType<MenuType>>();
        }
    }
}