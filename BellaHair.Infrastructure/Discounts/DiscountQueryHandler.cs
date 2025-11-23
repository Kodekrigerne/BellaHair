using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Ports.Discounts;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Discounts
{
    public class DiscountQueryHandler : IDiscountQuery
    {
        private readonly BellaHairContext _db;
        private readonly IDiscountCalculatorService _discountCalculatorService;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public DiscountQueryHandler(BellaHairContext db, IDiscountCalculatorService discountCalculatorService, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _db = db;
            _discountCalculatorService = discountCalculatorService;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        async Task<BookingDiscountDTO?> IDiscountQuery.FindBestDiscount(FindBestDiscountQuery query)
        {
            var employee = await _db.Employees.AsNoTracking().Include(e => e.Treatments).SingleOrDefaultAsync(e => e.Id == query.EmployeeId)
                ?? throw new KeyNotFoundException($"Could not find employee with Id {query.EmployeeId}");
            var customer = await _db.PrivateCustomers.AsNoTracking().Include(c => c.Bookings).SingleOrDefaultAsync(c => c.Id == query.CustomerId) //TODO: Fix dette når vi har valgt en Visits strategi
                ?? throw new KeyNotFoundException($"Could not find customer with Id {query.CustomerId}");
            var treatment = await _db.Treatments.AsNoTracking().SingleOrDefaultAsync(t => t.Id == query.TreatmentId)
                ?? throw new KeyNotFoundException($"Could not find treatment with Id {query.TreatmentId}");

            //Det er nødvendigt at oprette en booking for at finde en rabat
            //Denne booking gemmes dog ikke
            var booking = Booking.Create(customer, employee, treatment, query.StartDateTime, _currentDateTimeProvider);

            var discount = await _discountCalculatorService.GetBestDiscount(booking);

            return discount == null ? null : new BookingDiscountDTO(discount.Name, discount.Amount);
        }
    }
}
