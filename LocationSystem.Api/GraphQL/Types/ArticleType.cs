using HotChocolate.Types;
using LocationSystem.Api.GraphQL.DataLoaders;
using LocationSystem.Domain.Entities.Articles;
using LocationSystem.Domain.Entities.UserRolePermissions;
using Microsoft.EntityFrameworkCore;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocationSystem.Application.Dtos.Articles;
using LocationSystem.Application.Dtos.Users;

namespace LocationSystem.Api.GraphQL.Types
{
    public class ArticleType : ObjectType<ArticleDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleDto> descriptor)
        {
            descriptor.Name("AppArticleDto");
            descriptor.Field(a => a.Id).Type<IdType>();
            descriptor.Field(a => a.CreateTime).Type<DateTimeType>();
            descriptor.Field(a => a.Content).Type<StringType>();
            descriptor.Field(a => a.Title).Type<StringType>();
            descriptor.Field(a => a.Subtitle).Type<StringType>();
            descriptor.Field(a => a.IsVisiable).Type<BooleanType>();
            descriptor.Field(a => a.UserId).Type<IdType>();
            descriptor.Field(a => a.Topic).Type<StringType>();
            
            // 使用 DataLoader 加载标签
            descriptor.Field(a => a.Tags)
                .Type<ListType<TagType>>()
                .Resolve(async ctx => {
                    var article = ctx.Parent<ArticleDto>();
                    var dataLoader = ctx.Service<ArticleTagsDataLoader>();
                    var tags = await dataLoader.LoadAsync(article.Id);
                    if (tags == null) return null;
                    return tags.Adapt<IEnumerable<TagDto>>();
                });
            
            // 使用 DataLoader 加载评论
            descriptor.Field(a => a.Comments)
                .Type<ListType<ArticleCommentType>>()
                .Resolve(async ctx => {
                    var article = ctx.Parent<ArticleDto>();
                    var dataLoader = ctx.Service<ArticleCommentsDataLoader>();
                    var comments = await dataLoader.LoadAsync(article.Id);
                    if (comments == null) return null;
                    return comments.Adapt<IEnumerable<ArticleCommentDto>>();
                });
            
            // 使用 DataLoader 加载创建者
            descriptor.Field(a => a.CreateUser)
                .Type<LocationSystem.Api.GraphQL.Types.UserType>()
                .Resolve(async ctx => {
                    var article = ctx.Parent<ArticleDto>();
                    var dataLoader = ctx.Service<ArticleCreateUserDataLoader>();
                    var user = await dataLoader.LoadAsync(article.UserId);
                    if (user == null) return null;
                    return user.Adapt<UserDto>();
                });
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
            descriptor.Field(c => c.CreateTime).Type<DateTimeType>();
        }
    }


}