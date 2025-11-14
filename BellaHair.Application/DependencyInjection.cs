using BellaHair.Application.Discounts;
using BellaHair.Ports.Discounts;
using BellaHair.Ports.Treatments;
using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ILoyaltyDiscountCommand, LoyaltyDiscountCommandHandler>();
            serviceCollection.AddScoped<ITreatmentCommand, TreatmentCommandHandler>();

            return serviceCollection;
        }
    }
}
