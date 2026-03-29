using HotChocolate.Types;
using LocationSystem.Application.Dtos;
using LocationSystem.Api.GraphQL.DataLoaders;
using LocationSystem.Domain.Entities.Articles;
using LocationSystem.Domain.Entities.UserRolePermissions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            
            // 使用 DataLoader 加载标签
            descriptor.Field(a => a.Tags)
                .Type<ListType<TagType>>()
                .ResolveWith<Resolvers>(r => r.GetTags(default!, default!));
            
            // 使用 DataLoader 加载评论
            descriptor.Field(a => a.Comments)
                .Type<ListType<ArticleCommentType>>()
                .ResolveWith<Resolvers>(r => r.GetComments(default!, default!));
            
            // 使用 DataLoader 加载创建者
            descriptor.Field(a => a.CreateUser)
                .Type<LocationSystem.Api.GraphQL.Types.UserType>()
                .ResolveWith<Resolvers>(r => r.GetCreateUser(default!, default!));
        }
        
        private class Resolvers
        {
            public async Task<IEnumerable<TagDto>> GetTags(
                ArticleDto article,
                ArticleTagsDataLoader dataLoader)
            {
                var tags = await dataLoader.LoadAsync(article.Id);
                // 转换为 TagDto
                return tags.Select(t => new TagDto { Id = t.Id, Name = t.Name });
            }
            
            public async Task<IEnumerable<ArticleCommentDto>> GetComments(
                ArticleDto article,
                ArticleCommentsDataLoader dataLoader)
            {
                var comments = await dataLoader.LoadAsync(article.Id);
                // 转换为 ArticleCommentDto
                return comments.Select(c => new ArticleCommentDto {
                    Id = c.Id,
                    UserId = c.UserId,
                    Comment = c.Comment,
                    IsVisiable = c.IsVisiable,
                    CreateTiem = c.CreateTiem
                });
            }
            
            public async Task<UserDto> GetCreateUser(
                ArticleDto article,
                ArticleCreateUserDataLoader dataLoader)
            {
                var user = await dataLoader.LoadAsync(article.UserId);
                if (user == null) return null;
                // 转换为 UserDto
                return new UserDto {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email?.ToString() ?? string.Empty,
                    UserType = user.UserType.ToString()
                };
            }
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