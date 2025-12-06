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

        async Task<bool> IBookingOverlapChecker.OverlapsWithBooking(DateTime startDateTime, int durationMinutes, Guid employeeId, Guid customerId, Guid? bookingId)
        {
            if (await CheckEmployeeOverlap(startDateTime, durationMinutes, employeeId, bookingId)) return true;
            if (await CheckCustomerOverlap(startDateTime, durationMinutes, customerId, bookingId)) return true;

            return false;
        }

        private async Task<bool> CheckEmployeeOverlap(DateTime startDateTime, int durationMinutes, Guid employeeId, Guid? bookingId = null)
        {
            var endDateTime = startDateTime.AddMinutes(durationMinutes);

            //TODO: Hvis vi indfører multi-treatment bookings skal vi finde ud af hvordan vi håndterer dette

            var bookings = _db.Bookings
                .AsNoTracking()
                .Where(b => b.EndDateTime > _currentDateTimeProvider.GetCurrentDateTime())
                .Where(b => b.Employee!.Id == employeeId && b.Id != bookingId);

            //ny starter < eksisterende slutter && eksisterende starter < ny slutter
            bool overlap = await bookings.AnyAsync(b =>
                startDateTime < b.EndDateTime &&
                b.StartDateTime < endDateTime
            );

            return overlap;
        }

        private async Task<bool> CheckCustomerOverlap(DateTime startDateTime, int durationMinutes, Guid customerId, Guid? bookingId = null)
        {
            var endDateTime = startDateTime.AddMinutes(durationMinutes);

            var bookings = _db.Bookings
                .AsNoTracking()
                .Where(b => b.EndDateTime > _currentDateTimeProvider.GetCurrentDateTime())
                .Where(b => b.Customer!.Id == customerId && b.Id != bookingId);

            //ny starter < eksisterende slutter && eksisterende starter < ny slutter
            bool overlap = await bookings.AnyAsync(b =>
                startDateTime < b.EndDateTime &&
                b.StartDateTime < endDateTime
            );

            return overlap;
        }
    }
}
