using HotChocolate.Types;

namespace LocationSystem.Api.GraphQL.Types
{
    public class SuccessResponseType : ObjectType<SuccessResponse>
    {
        protected override void Configure(IObjectTypeDescriptor<SuccessResponse> descriptor)
        {
            descriptor.Field(r => r.Success).Type<NonNullType<BooleanType>>();
        }
    }

    public class SuccessResponse
    {
        public bool Success { get; set; }
    }
}