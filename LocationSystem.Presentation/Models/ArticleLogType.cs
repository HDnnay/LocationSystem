using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Presentation.Models
{
    public class ArticleLogType : ObjectType<ArticleLogGraphqLDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleLogGraphqLDto> descriptor)
        {
            descriptor.Name("articleLog");
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>().Description("日志ID");
            descriptor.Field(t => t.ArticleId).Type<IdType>().Description("文章ID");
            descriptor.Field(t => t.UserId).Type<NonNullType<IdType>>().Description("用户ID");
            descriptor.Field(t => t.State).Type<NonNullType<EnumType<ArticleState>>>().Description("文章状态");
            descriptor.Field(t => t.Log).Type<NonNullType<StringType>>().Description("日志内容");
            descriptor.Field(t => t.CreateTime).Type<NonNullType<DateTimeType>>().Description("创建时间");
        }
    }
}