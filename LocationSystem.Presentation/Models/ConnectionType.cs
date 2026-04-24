using HotChocolate.Types.Pagination;

namespace LocationSystem.Presentation.Models
{
    public class ConnectionType<T> : ObjectType<Connection<T>> where T : class
    {
        protected override void Configure(IObjectTypeDescriptor<Connection<T>> descriptor)
        {
            descriptor.Field(c => c.Edges)
                .Type<ListType<EdgeType<T>>>();

            descriptor.Field(c => c.Info)
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
}
