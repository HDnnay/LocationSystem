using LocationSystem.Application.GrapqLDTOs.Roles;

namespace LocationSystem.Presentation.Models
{
    public class RoleType : ObjectType<RoleGraphqLDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RoleGraphqLDto> descriptor)
        {
            descriptor.Name("role");
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>().Description("角色ID");
            descriptor.Field(t => t.Name).Type<StringType>().Description("角色名称");
            descriptor.Field(t => t.Code).Type<StringType>().Description("代码");
            descriptor.Field(t => t.IsDisabled).Type<BooleanType>().Description("是否禁用");
            descriptor.Field(t => t.Description).Type<StringType>().Description("角色描述");
            descriptor.Field(t => t.CreatedAt).Type<DateTimeType>().Description("创建时间");
            descriptor.Field(t => t.UpdatedAt).Type<DateTimeType>().Description("更新时间");
        }
    }
}
