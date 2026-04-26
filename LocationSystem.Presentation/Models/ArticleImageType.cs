using LocationSystem.Application.GrapqLDTOs.Articles;

namespace LocationSystem.Presentation.Models
{
    public class ArticleImageType : ObjectType<ArticleImageGraphqLDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleImageGraphqLDto> descriptor)
        {
            descriptor.Name("articleImage");
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>().Description("图片ID");
            descriptor.Field(t => t.ArticelId).Type<IdType>().Description("文章ID");
            descriptor.Field(t => t.ImagePaths).Type<StringType>().Description("图片路径");
            descriptor.Field(t => t.CreateTime).Type<NonNullType<DateTimeType>>().Description("创建时间");
            descriptor.Field(t => t.IsDelete).Type<NonNullType<BooleanType>>().Description("是否删除");
            descriptor.Field(t => t.DeleteUserId).Type<IdType>().Description("删除人ID");
            descriptor.Field(t => t.DeleteTime).Type<DateTimeType>().Description("删除时间");
            descriptor.Field(t => t.IsDisabled).Type<NonNullType<BooleanType>>().Description("是否禁用");
        }
    }
}