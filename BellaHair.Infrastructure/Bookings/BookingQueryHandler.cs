using BellaHair.Domain;
using BellaHair.Ports.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Bookings
{
    internal class BookingQueryHandler : IBookingQueryHandler
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public BookingQueryHandler(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        async Task<IEnumerable<BookingSimpleDTO>> IBookingQueryHandler.GetAllAsync()
        {
            var bookings = await _db.Bookings
                .Include(b => b.Treatment)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .ToListAsync();

            //TODO: Vælg mellem denne og nedenstående
            return bookings.Select(b => new BookingSimpleDTO(
                b.StartDateTime,
                b.Total,
                b.Employee?.Name.FullName ?? b.EmployeeSnapshot.FullName,
                b.Customer?.Name.FullName ?? b.CustomerSnapshot.FullName,
                b.Treatment?.Name ?? b.TreatmentSnapshot.Name,
                b.TreatmentSnapshot.DurationMinutes, //if (IsPaid) Snapshot, ellers relation
                b.Discount != null ? new DiscountDTO(b.Discount.Name, b.Discount.Amount) : null
                ));

            //return await _db.Bookings.Select(b => new BookingSimpleDTO(
            //    b.StartDateTime,
            //    b.Total,
            //    b.EmployeeSnapshot.FullName,
            //    b.CustomerSnapshot.FullName,
            //    b.TreatmentSnapshot.Name,
            //    b.TreatmentSnapshot.DurationMinutes,
            //    b.Discount != null ? new DiscountDTO(b.Discount.Name, b.Discount.Amount) : null
            //    )).ToListAsync();
        }
    }
}
