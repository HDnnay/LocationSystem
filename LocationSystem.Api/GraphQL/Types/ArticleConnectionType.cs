using HotChocolate.Types;
using LocationSystem.Application.Utilities.Pagination;
using LocationSystem.Domain.Entities.Articles;

namespace LocationSystem.Api.GraphQL.Types
{
    public class ArticleConnectionType : ObjectType<Connection<Article>>
    {
        protected override void Configure(IObjectTypeDescriptor<Connection<Article>> descriptor)
        {
            descriptor.Name("ArticlesConnection");
            
            descriptor.Field(c => c.Edges)
                .Type<ListType<ArticleEdgeType>>();
                
            descriptor.Field(c => c.PageInfo)
                .Type<ArticlePageInfoType>();
        }
    }

    public class ArticleEdgeType : ObjectType<Edge<Article>>
    {
        protected override void Configure(IObjectTypeDescriptor<Edge<Article>> descriptor)
        {
            descriptor.Name("ArticlesEdge");
            
            descriptor.Field(e => e.Cursor)
                .Type<NonNullType<StringType>>();
                
            descriptor.Field(e => e.Node)
                .Type<NonNullType<ArticleType>>();
        }
    }

    public class ArticlePageInfoType : ObjectType<PageInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<PageInfo> descriptor)
        {
            descriptor.Name("ArticlesPageInfo");
            
            descriptor.Field(p => p.HasNextPage)
                .Type<NonNullType<BooleanType>>();
                
            descriptor.Field(p => p.HasPreviousPage)
                .Type<NonNullType<BooleanType>>();
                
            descriptor.Field(p => p.StartCursor)
                .Type<StringType>();
                
            descriptor.Field(p => p.EndCursor)
                .Type<StringType>();
        }
    }
}
