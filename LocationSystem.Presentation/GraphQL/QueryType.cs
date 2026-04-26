using LocationSystem.Presentation.Extensions;
using LocationSystem.Presentation.Models;

namespace LocationSystem.Presentation.GraphQL
{

    public class QueryType : ObjectType<Query>
    {

        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Name("Query");
            descriptor.Field(q => q.GetArticles(default!))
                .Type<ListType<ArticleType>>()
                .WithPermission("article.list.read")
                .Description("获取文章列表，支持过滤和排序");
        }
    }

}
