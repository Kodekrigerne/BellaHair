using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;

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


        public Task<Booking> GetAsync(Guid id)
        {
            
        }

        public Task<List<Booking>> GetFutureBookings()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
