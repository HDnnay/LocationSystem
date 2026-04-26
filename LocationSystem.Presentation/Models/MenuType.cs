using LocationSystem.Application.GrapqLDTOs.Menus;
using LocationSystem.Presentation.DataLoaders;

namespace LocationSystem.Presentation.Models
{
    public class MenuType : ObjectType<MenuGraphqLDto>
    {
        protected override void Configure(IObjectTypeDescriptor<MenuGraphqLDto> descriptor)
        {
            descriptor.Name("menu");
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>().Description("菜单Id");
            descriptor.Field(t => t.Name).Type<StringType>().Description("菜单名称");
            descriptor.Field(t => t.Icon).Type<StringType>().Description("菜单图标");
            descriptor.Field(t => t.Path).Type<StringType>().Description("菜单路径");
            descriptor.Field(t => t.IsBackEnd).Type<BooleanType>().Description("后端菜单");
            descriptor.Field(t => t.Level).Type<IntType>().Description("菜单层级");
            descriptor.Field(t => t.ParentId).Type<StringType>().Description("父级菜单Id");
            descriptor.Field(t => t.CreatedAt).Type<DateTimeType>().Description("创建时间");
            descriptor.Field(t => t.UpdatedAt).Type<DateTimeType>().Description("更新时间");
            descriptor.Field("parent").Type<MenuType>().Description("父级菜单").Resolve(async context =>
            {
                var menu = context.Parent<MenuGraphqLDto>();
                if (menu.ParentId == null)
                    return null;

                // 使用 DataLoader 批量加载父级菜单
                var dataLoader = context.DataLoader<MenuDataLoader>();
                var parentMenu = await dataLoader.LoadAsync(menu.ParentId.Value, context.RequestAborted);
                return parentMenu;
            });
        }
    }
}
