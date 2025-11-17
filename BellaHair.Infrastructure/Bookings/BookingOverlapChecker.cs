using BellaHair.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Bookings
{
    public class BookingOverlapChecker : IBookingOverlapChecker
    {
        private readonly BellaHairContext _db;

        public BookingOverlapChecker(BellaHairContext db) => _db = db;

        async Task<bool> IBookingOverlapChecker.CheckOverlap(Guid employeeId, DateTime startDateTime, int durationMinutes)
        {
            if (!await _db.Employees.AnyAsync(e => e.Id == employeeId))
                throw new KeyNotFoundException($"No employee with ID {employeeId} exists");

            var bookings = _db.Bookings
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .Where(b => b.Employee!.Id == employeeId);

            if (await bookings.AnyAsync(b => startDateTime > b.StartDateTime && startDateTime < b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value))) return true;
            if (await bookings.AnyAsync(b => startDateTime.AddMinutes(durationMinutes) > b.StartDateTime && startDateTime.AddMinutes(durationMinutes) < b.StartDateTime.AddMinutes(b.Treatment!.DurationMinutes.Value))) return true;

            return false;
        }
    }
}
