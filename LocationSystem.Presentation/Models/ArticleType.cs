using LocationSystem.Application.GrapqLDTOs.Articles;
using LocationSystem.Domain.Enums;
using LocationSystem.Presentation.DataLoaders;

namespace LocationSystem.Presentation.Models
{
    public class ArticleType : ObjectType<ArticleGraphqLDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleGraphqLDto> descriptor)
        {
            descriptor.Name("article");
            descriptor.Field(t => t.Id).Type<NonNullType<IdType>>().Description("文章ID");
            descriptor.Field(t => t.Title).Type<NonNullType<StringType>>().Description("标题");
            descriptor.Field(t => t.Content).Type<NonNullType<StringType>>().Description("内容");
            descriptor.Field(t => t.Subtitle).Type<StringType>().Description("副标题");
            descriptor.Field(t => t.Topic).Type<StringType>().Description("主题");
            descriptor.Field(t => t.UserId).Type<NonNullType<IdType>>().Description("作者ID");
            descriptor.Field(t => t.CreateTime).Type<NonNullType<DateTimeType>>().Description("创建时间");
            descriptor.Field(t => t.DeleteUserId).Type<IdType>().Description("删除人ID");
            descriptor.Field(t => t.DeleteTime).Type<DateTimeType>().Description("删除时间");
            descriptor.Field(t => t.IsDelete).Type<NonNullType<BooleanType>>().Description("是否删除");
            descriptor.Field(t => t.IsDisabled).Type<NonNullType<BooleanType>>().Description("是否禁用");
            descriptor.Field(t => t.Level).Type<NonNullType<EnumType<ArticleLevel>>>().Description("文章可见等级");
            descriptor.Field(t => t.VisibleStartTime).Type<DateTimeType>().Description("文章可见开始时间");
            descriptor.Field(t => t.VisibleEndTime).Type<DateTimeType>().Description("文章可见结束时间");
            descriptor.Field("creator").Type<UserType>().Description("作者").Resolve(async context =>
            {
                var article = context.Parent<ArticleGraphqLDto>();
                var dataLoader = context.DataLoader<UserDataLoader>();
                var user = await dataLoader.LoadAsync(article.UserId, context.RequestAborted);
                return user;
            });
            descriptor.Field("comments").Type<ListType<ArticleCommentType>>().Description("评论列表").Resolve(async context =>
            {
                var article = context.Parent<ArticleGraphqLDto>();
                var dataLoader = context.DataLoader<ArticleCommentDataLoader>();
                var comments = await dataLoader.LoadAsync(article.Id, context.RequestAborted);
                return comments.ToList(); // GroupDataLoader 返回 IEnumerable，需要转换为 List
            });
            descriptor.Field("tags").Type<ListType<ArticleTagType>>().Description("标签列表").Resolve(async context =>
            {
                var article = context.Parent<ArticleGraphqLDto>();
                var dataLoader = context.DataLoader<ArticleTagDataLoader>();
                var tags = await dataLoader.LoadAsync(article.Id, context.RequestAborted);
                return tags.ToList(); // GroupDataLoader 返回 IEnumerable，需要转换为 List
            });
        }
    }
}
