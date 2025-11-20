using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Treatments;

namespace BellaHair.Domain.Bookings
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
        void Delete(Booking booking);
        Task<Booking> GetAsync(Guid id);
        Task SaveChangesAsync();

    }
}
