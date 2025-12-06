using BellaHair.Application;
using BellaHair.Application.Discounts;
using BellaHair.Application.Employees;
using BellaHair.Application.Invoices;
using BellaHair.Application.PrivateCustomers;
using BellaHair.Application.Products;
using BellaHair.Application.Treatments;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.Invoices;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Products;
using BellaHair.Domain.Treatments;
using BellaHair.Infrastructure;
using BellaHair.Infrastructure.Bookings;
using BellaHair.Infrastructure.Discounts;
using BellaHair.Infrastructure.Employees;
using BellaHair.Infrastructure.Invoices;
using BellaHair.Infrastructure.PrivateCustomers;
using BellaHair.Infrastructure.Products;
using BellaHair.Infrastructure.Treatments;
using BellaHair.Ports.Bookings;
using BellaHair.Ports.Discounts;
using BellaHair.Ports.Employees;
using BellaHair.Ports.Invoices;
using BellaHair.Ports.PrivateCustomers;
using BellaHair.Ports.Products;
using BellaHair.Ports.Treatments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace CrossCut
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBellaHairContext(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<BellaHairContext>(options =>
                options.UseSqlite(connectionString));

            return serviceCollection;
        }

        public static IServiceCollection AddDbConfigure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<DbConfigure>();

            return serviceCollection;
        }

        public static IServiceCollection AddDataProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<DataProvider>();

            return serviceCollection;
        }

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

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDiscountQuery, DiscountQueryHandler>();

            serviceCollection.AddScoped<IDiscountCalculatorService, DiscountCalculatorService>();
            serviceCollection.AddScoped<ILoyaltyDiscountRepository, LoyaltyDiscountRepository>();
            serviceCollection.AddScoped<ILoyaltyDiscountQuery, LoyaltyDiscountQueryHandler>();
            serviceCollection.AddScoped<ICampaignDiscountRepository, CampaignDiscountRepository>();
            serviceCollection.AddScoped<ICampaignDiscountQuery, CampaignDiscountQueryHandler>();
            serviceCollection.AddScoped<IBirthdayDiscountQuery, BirthdayDiscountQueryHandler>();

            serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
            serviceCollection.AddScoped<IEmployeeQuery, EmployeeQueryHandler>();
            serviceCollection.AddScoped<IEmployeeFutureBookingsChecker, EmployeeFutureBookingsChecker>();

            serviceCollection.AddScoped<ITreatmentRepository, TreatmentRepository>();
            serviceCollection.AddScoped<ITreatmentQuery, TreatmentQueryHandler>();
            serviceCollection.AddScoped<ITreatmentDuplicateChecker, TreatmentDuplicateChecker>();

            serviceCollection.AddScoped<IPrivateCustomerRepository, PrivateCustomerRepository>();
            serviceCollection.AddScoped<IPrivateCustomerQuery, PrivateCustomerQueryHandler>();
            serviceCollection.AddScoped<IPCustomerFutureBookingChecker, PCustomerFutureBookingChecker>();
            serviceCollection.AddScoped<ICustomerVisitsService, CustomerVisitsService>();
            serviceCollection.AddScoped<ICustomerOverlapChecker, CustomerOverlapChecker>();

            serviceCollection.AddScoped<ICurrentDateTimeProvider, CurrentDateTimeProvider>();

            serviceCollection.AddScoped<IBookingQuery, BookingQueryHandler>();
            serviceCollection.AddScoped<IBookingRepository, BookingRepository>();
            serviceCollection.AddScoped<IBookingOverlapChecker, BookingOverlapChecker>();
            serviceCollection.AddScoped<IFutureBookingWithTreatmentChecker, FutureBookingWithTreatmentChecker>();

            serviceCollection.AddScoped<IInvoiceQuery, InvoiceQueryHandler>();

            serviceCollection.AddScoped<IInvoiceRepository, InvoiceRepository>();

            serviceCollection.AddScoped<IEmailService, EmailService>();

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

            serviceCollection.AddScoped<IProductQuery, ProductQueryHandler>();
            serviceCollection.AddScoped<IProductRepository, ProductRepository>();

            return serviceCollection;
        }
    }
}
