using LocationSystem.Application.GrapqLDTOs.Permissons;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Presentation.Models
{
    public class PermissionType : ObjectType<PermissionGraphqLDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PermissionGraphqLDto> descriptor)
        {
            descriptor.Name("permission");
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>().Description("权限Id");
            descriptor.Field(t => t.Name).Type<StringType>().Description("权限名称");
            descriptor.Field(t => t.Code).Type<StringType>().Description("代码");
            descriptor.Field(t => t.IsDisabled).Type<BooleanType>().Description("是否禁用");
            descriptor.Field(t => t.Description).Type<StringType>().Description("权限描述");
            descriptor.Field(t => t.ParentId).Type<StringType>().Description("父级权限Id");
            descriptor.Field(t => t.CreatedAt).Type<DateTimeType>().Description("创建时间");
            descriptor.Field(t => t.UpdatedAt).Type<DateTimeType>().Description("更新时间");
            descriptor.Field("parent").Type<PermissionType>().Description("父级权限").Resolve(async context =>
            {
                var permission = context.Parent<PermissionGraphqLDto>();
                if (permission.ParentId == null)
                    return null;

                // 这里需要实现加载父级权限的逻辑
                var mediator = context.Service<IMediator>();
            });
        }
    }
}
