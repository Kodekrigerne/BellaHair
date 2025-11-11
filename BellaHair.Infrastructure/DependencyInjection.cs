using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Infrastructure.Discounts;
using BellaHair.Ports.Discounts;
using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<BellaHairContext>();
            serviceCollection.AddScoped<IDiscountCalculatorService, DiscountCalculatorService>();
            serviceCollection.AddScoped<ILoyaltyDiscountRepository, LoyaltyDiscountRepository>();
            serviceCollection.AddScoped<ILoyaltyDiscountQuery, LoyaltyDiscountQueryHandler>();

            return serviceCollection;
        }
    }
}
