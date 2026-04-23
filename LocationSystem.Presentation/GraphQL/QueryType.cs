using LocationSystem.Application.Features.Users.Queries;
using LocationSystem.Application.GrapqLDTOs;
using LocationSystem.Application.Utilities;
using LocationSystem.Application.Utilities.Common;
using LocationSystem.Presentation.InputTypes;
using Mapster;

namespace LocationSystem.Presentation.GraphQL
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("Users").Argument("input", a => a.Type<UserQueryInputType>())
                .Type<PageResult<UserGrapqLDto>>()
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
