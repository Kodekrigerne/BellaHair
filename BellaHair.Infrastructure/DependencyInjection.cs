using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.Invoices;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;
using BellaHair.Infrastructure.Bookings;
using BellaHair.Infrastructure.Discounts;
using BellaHair.Infrastructure.Employees;
using BellaHair.Infrastructure.Invoices;
using BellaHair.Infrastructure.PrivateCustomers;
using BellaHair.Infrastructure.Treatments;
using BellaHair.Ports.Bookings;
using BellaHair.Ports.Discounts;
using BellaHair.Ports.Employees;
using BellaHair.Ports.Invoices;
using BellaHair.Ports.PrivateCustomers;
using BellaHair.Ports.Treatments;
using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDiscountQuery, DiscountQueryHandler>();

            serviceCollection.AddScoped<IDiscountCalculatorService, DiscountCalculatorService>();
            serviceCollection.AddScoped<ILoyaltyDiscountRepository, LoyaltyDiscountRepository>();
            serviceCollection.AddScoped<ILoyaltyDiscountQuery, LoyaltyDiscountQueryHandler>();
            serviceCollection.AddScoped<ICampaignDiscountRepository, CampaignDiscountRepository>();
            serviceCollection.AddScoped<ICampaignDiscountQuery, CampaignDiscountQueryHandler>();

            serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
            serviceCollection.AddScoped<IEmployeeQuery, EmployeeQueryHandler>();
            serviceCollection.AddScoped<IEmployeeFutureBookingsChecker, EmployeeFutureBookingsChecker>();

            serviceCollection.AddScoped<ITreatmentRepository, TreatmentRepository>();
            serviceCollection.AddScoped<ITreatmentQuery, TreatmentQueryHandler>();

            serviceCollection.AddScoped<IPrivateCustomerRepository, PrivateCustomerRepository>();
            serviceCollection.AddScoped<IPrivateCustomerQuery, PrivateCustomerQueryHandler>();
            serviceCollection.AddScoped<IPCustomerFutureBookingChecker, PCustomerFutureBookingChecker>();
            serviceCollection.AddScoped<ICustomerVisitsService, CustomerVisitsService>();

            serviceCollection.AddScoped<ICurrentDateTimeProvider, CurrentDateTimeProvider>();

            serviceCollection.AddScoped<IBookingQuery, BookingQueryHandler>();
            serviceCollection.AddScoped<IBookingRepository, BookingRepository>();
            serviceCollection.AddScoped<IBookingOverlapChecker, BookingOverlapChecker>();
            serviceCollection.AddScoped<IFutureBookingWithTreatmentChecker, FutureBookingWithTreatmentChecker>();

            serviceCollection.AddScoped<IInvoiceQuery, InvoiceQueryHandler>();

            serviceCollection.AddScoped<InvoiceDocumentDataSource>();

            serviceCollection.AddScoped<IInvoiceRepository, InvoiceRepository>();
            serviceCollection.AddScoped<IInvoiceDocumentDataSource, InvoiceDocumentDataSource>();
            serviceCollection.AddScoped<IInvoiceChecker, InvoiceChecker>();

            return serviceCollection;
        }
    }
}
