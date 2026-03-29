using HotChocolate.Types;
using LocationSystem.Application.Utilities.Pagination;

namespace LocationSystem.Api.GraphQL.Types
{
    public class ConnectionType<T> : ObjectType<Connection<T>> where T : class
    {
        protected override void Configure(IObjectTypeDescriptor<Connection<T>> descriptor)
        {
            descriptor.Field(c => c.Edges)
                .Type<ListType<EdgeType<T>>>();
            
            descriptor.Field(c => c.PageInfo)
                .Type<PageInfoType>();
        }
    }

    public class EdgeType<T> : ObjectType<Edge<T>> where T : class
    {
        protected override void Configure(IObjectTypeDescriptor<Edge<T>> descriptor)
        {
            descriptor.Field(e => e.Cursor)
                .Type<NonNullType<StringType>>();
            
            descriptor.Field(e => e.Node)
                .Type<NonNullType<ObjectType<T>>>();
        }
    }

    public class PageInfoType : ObjectType<PageInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<PageInfo> descriptor)
        {
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
