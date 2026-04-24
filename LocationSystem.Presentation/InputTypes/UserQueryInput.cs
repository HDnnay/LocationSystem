namespace LocationSystem.Presentation.InputTypes
{
    public record UserQueryInput
    (
        string? KeyWord,
        int? Page = 1,
        int PageSize = 10,
        bool? FilterDelete = false
    );
    public class UserQueryInputType : InputObjectType<UserQueryInput>
    {

        protected override void Configure(IInputObjectTypeDescriptor<UserQueryInput> descriptor)
        {
            descriptor.Name("UserQueryInput");
            descriptor.Description("用户查询参数");
            descriptor.Field(t => t.Page).Description("页码");
            descriptor.Field(t => t.PageSize).Description("每页记录数");
            descriptor.Field(t => t.KeyWord).Description("关键词");
            descriptor.Field(t => t.FilterDelete).Description("是否过滤删除用户");


        }
    }
}
