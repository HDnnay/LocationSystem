namespace LocationSystem.Presentation.GraphQL
{
    public class QueryType : ObjectType<Query>
    {

        //protected override void Configure(IObjectTypeDescriptor descriptor)
        //{
        //    descriptor.Field("Users").UsePaging<UserType>()
        //        .Resolve(async context =>
        //        {
        //            var query = new GetUsersQuery();
        //            var result = await context.Service<IMediator>().Send(query);
        //            return result.Select(t => t.Adapt<UserType>());
        //        });
        //}
    }

}
