using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<BellaHairContext>();

            return serviceCollection;
        }
    }
}
