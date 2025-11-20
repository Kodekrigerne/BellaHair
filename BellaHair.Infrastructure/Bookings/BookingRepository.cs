using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.Bookings;

namespace BellaHair.Infrastructure.Bookings
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BellaHairContext _db;

        public BookingRepository(BellaHairContext db)
            => _db = db;

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
            return await _db.Bookings.FindAsync(id) ??
                throw new KeyNotFoundException($"Booking with id {id} is not found.");
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
