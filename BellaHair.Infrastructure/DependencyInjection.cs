using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Infrastructure.Discounts;
using BellaHair.Infrastructure.PrivateCustomers;
using BellaHair.Ports.Discounts;
using BellaHair.Ports.PrivateCustomers;
using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDiscountCalculatorService, DiscountCalculatorService>();
            serviceCollection.AddScoped<ILoyaltyDiscountRepository, LoyaltyDiscountRepository>();
            serviceCollection.AddScoped<ILoyaltyDiscountQuery, LoyaltyDiscountQueryHandler>();

            serviceCollection.AddScoped<IPrivateCustomerRepository, PrivateCustomerRepository>();
            serviceCollection.AddScoped<IPrivateCustomerQuery, PrivateCustomerQueryHandler>();


            return serviceCollection;
        }
    }
}
