using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Bookings
{
    // Mikkel Klitgaard
    /// <summary>
    /// Implements the booking repository for managing booking entities.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public BookingRepository(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        public async Task AddAsync(Booking booking)
        {
            await _db.Bookings.AddAsync(booking);
        }

        public void Delete(Booking booking)
        {
            _db.Bookings.Remove(booking);
        }

        public async Task<Booking> GetAsync(Guid id)
        {
            var now = _currentDateTimeProvider.GetCurrentDateTime();

            var booking = await _db.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .SingleOrDefaultAsync(b => b.Id == id) ??
                throw new KeyNotFoundException($"Booking with id {id} is not found.");

            if (booking.Customer != null)
            {
                var visits = await _db.PrivateCustomers.AsNoTracking().Where(c => c.Id == booking.Customer.Id).Select(c => c.Bookings.Count(b => b.EndDateTime < now)).SingleOrDefaultAsync();
                booking.Customer.SetVisits(visits);
            }

            return booking;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
