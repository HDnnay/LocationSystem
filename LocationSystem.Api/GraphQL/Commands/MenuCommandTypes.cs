using HotChocolate.Types;
using LocationSystem.Application.Features.Menus.Commands.CreateMenu;
using LocationSystem.Application.Features.Menus.Commands.UpdateMenu;

namespace LocationSystem.Api.GraphQL.Commands
{
    public class CreateMenuCommandType : InputObjectType<CreateMenuCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateMenuCommand> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Path).Type<StringType>();
            descriptor.Field(c => c.Icon).Type<StringType>();
            descriptor.Field(c => c.Order).Type<IntType>();
            descriptor.Field(c => c.Level).Type<IntType>();
            descriptor.Field(c => c.ParentId).Type<IdType>();
        }
    }

    public class UpdateMenuCommandType : InputObjectType<UpdateMenuCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateMenuCommand> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Path).Type<StringType>();
            descriptor.Field(c => c.Icon).Type<StringType>();
            descriptor.Field(c => c.Order).Type<IntType>();
            descriptor.Field(c => c.Level).Type<IntType>();
            descriptor.Field(c => c.ParentId).Type<IdType>();
        }
    }
}