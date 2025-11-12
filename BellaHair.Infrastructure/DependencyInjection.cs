using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Infrastructure.Discounts;
using BellaHair.Infrastructure.Employees;
using BellaHair.Ports.Discounts;
using BellaHair.Ports.Employees;
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

            serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
            serviceCollection.AddScoped<IEmployeeQuery, EmployeeQueryHandler>();

            return serviceCollection;
        }
    }
}
