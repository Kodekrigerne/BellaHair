using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Bookings
{
    //Dennis
    /// <inheritdoc cref="IBookingOverlapChecker"/>
    public class BookingOverlapChecker : IBookingOverlapChecker
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public BookingOverlapChecker(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        async Task<bool> IBookingOverlapChecker.CheckOverlap(Booking booking)
        {
            if (booking.Employee == null) throw new InvalidOperationException($"Booking must be loaded with employee, {booking.Id} overlap check");
            if (booking.Treatment == null) throw new InvalidOperationException($"Booking must be loaded with treatment, {booking.Id} overlap check");

            // Da vi har adgang til _db kan vi loade bookingen ind med relationer hvis de mangler fra den booking der passes ind
            if (booking.Employee == null || booking.Treatment == null)
            {
                booking = await _db.Bookings
                    .Include(b => b.Employee)
                    .Include(b => b.Treatment)
                    .FirstOrDefaultAsync(b => b.Id == booking.Id) ?? throw new KeyNotFoundException($"Booking not found {booking.Id}");
            }

            var bookings = _db.Bookings
                .AsNoTracking()
                .Where(b => b.StartDateTime > _currentDateTimeProvider.GetCurrentDateTime())
                .Where(b => b.Employee!.Id == booking.Employee!.Id)
                .Include(b => b.Employee)
                .Include(b => b.Treatment);

            // Er der nogen bookings for medarbejderen som starter før og slutter efter den nye bookings starttid?
            if (await bookings.AnyAsync(b => booking.StartDateTime > b.StartDateTime && booking.StartDateTime < b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value))) return true;
            // Er der nogen bookings for medarbejderen som starter før og slutter efter den nye bookings sluttid?
            if (await bookings.AnyAsync(b => booking.StartDateTime.AddMinutes(booking.Treatment!.DurationMinutes.Value) > b.StartDateTime && booking.StartDateTime.AddMinutes(booking.Treatment.DurationMinutes.Value) < b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value))) return true;

            return false;
        }
    }
}
