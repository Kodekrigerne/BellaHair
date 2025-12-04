using BellaHair.Application.Discounts;
using BellaHair.Application.Employees;
using BellaHair.Application.Invoices;
using BellaHair.Application.PrivateCustomers;
using BellaHair.Application.Products;
using BellaHair.Application.Treatments;
using BellaHair.Ports.Bookings;
using BellaHair.Ports.Discounts;
using BellaHair.Ports.Employees;
using BellaHair.Ports.Invoices;
using BellaHair.Ports.PrivateCustomers;
using BellaHair.Ports.Products;
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
            serviceCollection.AddScoped<IEmployeeCommand, EmployeeCommandHandler>();
            serviceCollection.AddScoped<IBookingCommand, BookingCommandHandler>();
            serviceCollection.AddScoped<ICampaignDiscountCommand, CampaignDiscountCommandHandler>();

            serviceCollection.AddScoped<IPrivateCustomerCommand, PrivateCustomerCommandHandler>();

            serviceCollection.AddScoped<IInvoiceCommand, InvoiceCommandHandler>();

            serviceCollection.AddScoped<IProductCommand, ProductCommandHandler>();

            return serviceCollection;
        }
    }
}
