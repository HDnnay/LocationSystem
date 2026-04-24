using LocationSystem.Application.Features.Users.Queries.GetUsers;
using LocationSystem.Application.Utilities;
using LocationSystem.Presentation.InputTypes;
using LocationSystem.Presentation.Models;
using Mapster;

namespace LocationSystem.Presentation.GraphQL
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("Users").UsePaging<UserType>()
                .Resolve(async context =>
                {
                    var input = context.ArgumentValue<UserQueryInput>("input");
                    var query = new GetUsersQuery();
                    var result = await context.Service<IMediator>().Send(query);
                    return result.Select(t => t.Adapt<UserType>());
                });
        }
    }

}
