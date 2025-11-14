using BellaHair.Application.Discounts;
using BellaHair.Application.PrivateCustomers;
using BellaHair.Ports.Discounts;
using BellaHair.Ports.PrivateCustomers;
using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ILoyaltyDiscountCommand, LoyaltyDiscountCommandHandler>();
            
            serviceCollection.AddScoped<IPrivateCustomerCommand, PrivateCustomerCommandHandler>();

            return serviceCollection;
        }
    }
}
