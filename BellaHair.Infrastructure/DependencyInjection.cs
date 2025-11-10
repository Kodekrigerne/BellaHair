using BellaHair.Domain.Bookings;
using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<BellaHairContext>();
            serviceCollection.AddScoped<IDiscountCalculatorService, DiscountCalculatorService>();

            return serviceCollection;
        }
    }
}
