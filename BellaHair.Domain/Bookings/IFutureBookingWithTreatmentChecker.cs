using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Bookings
{
    public interface IFutureBookingWithTreatmentChecker
    {
        Task<bool> CheckFutureBookingsWithTreatmentAsync(Guid id);
    }
}
