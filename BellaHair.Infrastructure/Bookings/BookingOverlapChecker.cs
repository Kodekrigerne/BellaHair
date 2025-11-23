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
            var bookings = _db.Bookings
                .AsNoTracking()
                .Where(b => b.StartDateTime > _currentDateTimeProvider.GetCurrentDateTime())
                .Where(b => b.Employee!.Id == employeeId);

            // Er der nogen bookings for medarbejderen som starter før og slutter efter den nye bookings starttid?
            if (await bookings.AnyAsync(b => startDateTime >= b.StartDateTime && startDateTime < b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value))) return true;
            // Er der nogen bookings for medarbejderen som starter før og slutter efter den nye bookings sluttid?
            if (await bookings.AnyAsync(b => startDateTime.AddMinutes(durationMinutes) >= b.StartDateTime && startDateTime.AddMinutes(durationMinutes) < b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value))) return true;

            return false;
        }

        private async Task<bool> CheckCustomerOverlap(DateTime startDateTime, int durationMinutes, Guid customerId)
        {
            var bookings = _db.Bookings
                .AsNoTracking()
                .Where(b => b.StartDateTime > _currentDateTimeProvider.GetCurrentDateTime())
                .Where(b => b.Customer!.Id == customerId);

            // Er der nogen bookings for medarbejderen som starter før og slutter efter den nye bookings starttid?
            if (await bookings.AnyAsync(b => startDateTime >= b.StartDateTime && startDateTime < b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value))) return true;
            // Er der nogen bookings for medarbejderen som starter før og slutter efter den nye bookings sluttid?
            if (await bookings.AnyAsync(b => startDateTime.AddMinutes(durationMinutes) >= b.StartDateTime && startDateTime.AddMinutes(durationMinutes) < b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value))) return true;

            return false;
        }
    }
}
