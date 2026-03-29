using HotChocolate.Types;
using LocationSystem.Application.Utilities;

namespace LocationSystem.Api.GraphQL.Types
{
    public class SuccessResponseType : ObjectType<SuccessResponse>
    {
        protected override void Configure(IObjectTypeDescriptor<SuccessResponse> descriptor)
        {
            descriptor.Field(r => r.Success).Type<NonNullType<BooleanType>>();
            descriptor.Field(r => r.Message).Type<StringType>();
        }
    }
}