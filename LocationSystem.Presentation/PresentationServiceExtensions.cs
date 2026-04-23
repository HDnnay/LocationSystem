using Microsoft.Extensions.DependencyInjection;

namespace LocationSystem.Presentation
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            // services
            //.AddGraphQLServer()
            //.AddQueryType<LocationQueries>()
            //.AddMutationType<LocationMutations>()
            //.AddSubscriptionType<LocationSubscriptions>()
            //.AddDataLoader<LocationDataLoaders>()
            //.AddDataLoader<DeviceDataLoaders>()
            //.AddFiltering()
            //.AddSorting()
            //.AddProjections()
            //.AddInMemorySubscriptions();

            return services;
        }
    }
}
