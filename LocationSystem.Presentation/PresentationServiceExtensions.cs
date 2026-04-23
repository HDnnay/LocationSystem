using LocationSystem.Presentation.GraphQL;
using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services
            .AddGraphQLServer()
            .AddQueryType<QueryType>()
            .AddMutationType<MutationType>();
            return services;
        }
    }
}
