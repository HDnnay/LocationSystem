using LocationSystem.Application.GrapqLDTOs.Articles;

namespace LocationSystem.Presentation.Models
{
    public class ArticleCommentType : ObjectType<ArticleCommentGraphqLDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleCommentGraphqLDto> descriptor)
        {
            descriptor.Name("articleComment");
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>().Description("评论Id");
            descriptor.Field(t => t.UserId).Type<NonNullType<IdType>>().Description("用户Id");
            descriptor.Field(t => t.Comment).Type<NonNullType<StringType>>().Description("评论内容");
            descriptor.Field(t => t.IsVisiable).Type<NonNullType<BooleanType>>().Description("是否可见");
            descriptor.Field(t => t.CreateTime).Type<NonNullType<DateTimeType>>().Description("创建时间");
        }
    }
}