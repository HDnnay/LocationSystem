using LocationSystem.Application.Features.Users.Queries;
using LocationSystem.Application.Utilities;
using LocationSystem.Presentation.InputTypes;
using Mapster;

namespace LocationSystem.Presentation.GraphQL
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("Users").Argument("input", a => a.Type<UserQueryInputType>())
                .Type<>()
                .Resolve(async context =>
                {
                    var input = context.ArgumentValue<UserQueryInput>("input");
                    var query = input.Adapt<GetAllUsersQuery>();
                    var result = await context.Service<IMediator>().Send(query);
                    return result;
                });
        }
    }

}
