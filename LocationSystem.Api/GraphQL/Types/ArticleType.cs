using HotChocolate.Types;
using LocationSystem.Application.Dtos;

namespace LocationSystem.Api.GraphQL.Types
{
    public class ArticleType : ObjectType<ArticleDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleDto> descriptor)
        {
            descriptor.Name("AppArticleDto");
            descriptor.Field(a => a.Id).Type<IdType>();
            descriptor.Field(a => a.CreateTiem).Type<DateTimeType>();
            descriptor.Field(a => a.Content).Type<StringType>();
            descriptor.Field(a => a.Title).Type<StringType>();
            descriptor.Field(a => a.Subtitle).Type<StringType>();
            descriptor.Field(a => a.IsVisiable).Type<BooleanType>();
            descriptor.Field(a => a.UserId).Type<IdType>();
            descriptor.Field(a => a.Topic).Type<StringType>();
            descriptor.Field(a => a.Tags).Type<ListType<TagType>>();
            descriptor.Field(a => a.Comments).Type<ListType<ArticleCommentType>>();
            descriptor.Field(a => a.CreateUser).Type<LocationSystem.Api.GraphQL.Types.UserType>();
        }
    }

    public class TagType : ObjectType<TagDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TagDto> descriptor)
        {
            descriptor.Name("AppTagDto");
            descriptor.Field(t => t.Id).Type<IdType>();
            descriptor.Field(t => t.Name).Type<StringType>();
        }
    }

    public class ArticleCommentType : ObjectType<ArticleCommentDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleCommentDto> descriptor)
        {
            descriptor.Name("AppArticleCommentDto");
            descriptor.Field(c => c.Id).Type<IdType>();
            descriptor.Field(c => c.UserId).Type<IdType>();
            descriptor.Field(c => c.Comment).Type<StringType>();
            descriptor.Field(c => c.IsVisiable).Type<BooleanType>();
            descriptor.Field(c => c.CreateTiem).Type<DateTimeType>();
        }
    }


}