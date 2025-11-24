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

        async Task<bool> IBookingOverlapChecker.OverlapsWithBooking(DateTime startDateTime, int durationMinutes, Guid employeeId, Guid customerId)
        {
            if (await CheckEmployeeOverlap(startDateTime, durationMinutes, employeeId)) return true;
            if (await CheckCustomerOverlap(startDateTime, durationMinutes, customerId)) return true;

            return false;
        }

        private async Task<bool> CheckEmployeeOverlap(DateTime startDateTime, int durationMinutes, Guid employeeId)
        {
            var endDateTime = startDateTime.AddMinutes(durationMinutes);

            var bookings = _db.Bookings
                .AsNoTracking()
                .Where(b => b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value) > _currentDateTimeProvider.GetCurrentDateTime())
                .Where(b => b.Employee!.Id == employeeId);

            //ny starter < eksisterende slutter && eksisterende starter < ny slutter
            bool overlap = await bookings.AnyAsync(b =>
                startDateTime < b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value) &&
                b.StartDateTime < endDateTime
            );

            return overlap;
        }

        private async Task<bool> CheckCustomerOverlap(DateTime startDateTime, int durationMinutes, Guid customerId)
        {
            var endDateTime = startDateTime.AddMinutes(durationMinutes);

            var bookings = _db.Bookings
                .AsNoTracking()
                .Where(b => b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value) > _currentDateTimeProvider.GetCurrentDateTime())
                .Where(b => b.Customer!.Id == customerId);

            //ny starter < eksisterende slutter && eksisterende starter < ny slutter
            bool overlap = await bookings.AnyAsync(b =>
                startDateTime < b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value) &&
                b.StartDateTime < endDateTime
            );

            return overlap;
        }
    }
}
