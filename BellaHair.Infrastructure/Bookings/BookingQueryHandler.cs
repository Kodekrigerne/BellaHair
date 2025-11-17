using BellaHair.Ports.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Bookings
{
    internal class BookingQueryHandler : IBookingQueryHandler
    {
        private readonly BellaHairContext _db;

        public BookingQueryHandler(BellaHairContext db) => _db = db;

        async Task<IEnumerable<BookingSimpleDTO>> IBookingQueryHandler.GetAllAsync()
        {
            return await _db.Bookings.Select(b => new BookingSimpleDTO(
                b.StartDateTime,
                b.Total,
                b.EmployeeSnapshot.FullName,
                b.CustomerSnapshot.FullName,
                b.TreatmentSnapshot.Name,
                b.TreatmentSnapshot.DurationMinutes,
                b.Discount != null ? new DiscountDTO(b.Discount.Name, b.Discount.Amount) : null
                )).ToListAsync();
        }
    }
}
