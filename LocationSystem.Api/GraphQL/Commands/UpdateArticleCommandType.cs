using HotChocolate.Types;
using LocationSystem.Application.Features.Articles.Commands.UpdateArticle;

namespace LocationSystem.Api.GraphQL.Commands
{
    public class UpdateArticleCommandType : InputObjectType<UpdateArticleCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateArticleCommand> descriptor)
        {
            descriptor.Name("UpdateArticleCommand");
            descriptor.Field(c => c.Title).Type<NonNullType<StringType>>().Description("文章标题");
            descriptor.Field(c => c.Content).Type<NonNullType<StringType>>().Description("文章内容");
            descriptor.Field(c => c.IsVisiable).Type<NonNullType<BooleanType>>().Description("是否可见");
            descriptor.Field(c => c.Topic).Type<StringType>().Description("文章主题");
            descriptor.Field(c => c.Subtitle).Type<StringType>().Description("文章副标题");
            descriptor.Field(c => c.TagIds).Type<ListType<IdType>>().Description("标签ID列表");
        }
    }
}
