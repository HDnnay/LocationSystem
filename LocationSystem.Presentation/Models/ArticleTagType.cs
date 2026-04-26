using LocationSystem.Application.GrapqLDTOs.Articles;

namespace LocationSystem.Presentation.Models
{
    public class ArticleTagType : ObjectType<ArticleTagGraphqLDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleTagGraphqLDto> descriptor)
        {
            descriptor.Name("articleTag");
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>().Description("标签ID");
            descriptor.Field(t => t.Name).Type<NonNullType<StringType>>().Description("标签名称");
            descriptor.Field(t => t.Description).Type<StringType>().Description("标签描述");
            descriptor.Field(t => t.IsVisiable).Type<NonNullType<BooleanType>>().Description("是否可见");
            descriptor.Field(t => t.CreateTime).Type<NonNullType<DateTimeType>>().Description("创建时间");
        }
    }
}