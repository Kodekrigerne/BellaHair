using BellaHair.Domain.Bookings;
using BellaHair.Infrastructure.PrivateCustomers;
using BellaHair.Ports.Bookings;
using BellaHair.Ports.Discounts;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace BellaHair.Infrastructure.Discounts
{
    //Dennis
    /// <inheritdoc cref="IDiscountQuery"/>
    public class DiscountQueryHandler : IDiscountQuery
    {
        private readonly BellaHairContext _db;
        private readonly IBookingRepository _bookingRepository;
        private readonly IDiscountCalculatorService _discountCalculatorService;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly ICustomerVisitsService _customerVisitsService;

        public DiscountQueryHandler(BellaHairContext db, IDiscountCalculatorService discountCalculatorService, ICurrentDateTimeProvider currentDateTimeProvider, ICustomerVisitsService customerVisitsService, IBookingRepository bookingRepository)
        {
            _db = db;
            _discountCalculatorService = discountCalculatorService;
            _currentDateTimeProvider = currentDateTimeProvider;
            _customerVisitsService = customerVisitsService;
            _bookingRepository = bookingRepository;
        }

        async Task<BookingDiscountDTO?> IDiscountQuery.FindBestDiscount(FindBestDiscountQuery query)
        {
            var now = _currentDateTimeProvider.GetCurrentDateTime();

            var employee = await _db.Employees.AsNoTracking().Include(e => e.Treatments).SingleOrDefaultAsync(e => e.Id == query.EmployeeId)
                ?? throw new KeyNotFoundException($"Could not find employee with Id {query.EmployeeId}");
            var customer = await _db.PrivateCustomers.AsNoTracking().SingleOrDefaultAsync(c => c.Id == query.CustomerId)
                ?? throw new KeyNotFoundException($"Could not find customer with Id {query.CustomerId}");
            var treatment = await _db.Treatments.AsNoTracking().SingleOrDefaultAsync(t => t.Id == query.TreatmentId)
                ?? throw new KeyNotFoundException($"Could not find treatment with Id {query.TreatmentId}");

            var visits = await _customerVisitsService.GetCustomerVisitsAsync(customer.Id);
            customer.SetVisits(visits);

            Booking? booking = null;
            if (query.StartDateTime < now && query.BookingId != null) booking = await _bookingRepository.GetAsync(query.BookingId.Value);
            else
            {
                var productLineDatas = await ConvertToProductLineData(query.ProductLines);
                booking = Booking.Create(customer, employee, treatment, query.StartDateTime, _currentDateTimeProvider, productLineDatas);
            }
            //Det er nødvendigt at oprette en booking for at finde en rabat
            //Denne booking gemmes dog ikke

            var discount = await _discountCalculatorService.GetBestDiscount(booking, query.IncludeBirthdayDiscount);

            return discount == null ? null : new BookingDiscountDTO(discount.Name, discount.Amount, (DiscountType)discount.Type);
        }


        private async Task<IEnumerable<ProductLineData>> ConvertToProductLineData(IEnumerable<CreateProductLine> createProductLines)
        {
            var productLineDatas = await Task.WhenAll(
                createProductLines.Select(async pl =>
                {
                    var quantity = Quantity.FromInt(pl.Quantity);
                    var product = await _db.Products.FindAsync(pl.ProductId)
                        ?? throw new KeyNotFoundException($"Product with ID {pl.ProductId} not found.");
                    return new ProductLineData(quantity, product);
                }));
            return productLineDatas;
        }
    }
}
