using BellaHair.Domain.PrivateCustomers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace BellaHair.Infrastructure.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary>
    /// Provides functionality to check whether a private customer has any future bookings.
    /// </summary>

    public class PCustomerFutureBookingChecker : IPCustomerFutureBookingChecker
    {
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly BellaHairContext _db;

        public PCustomerFutureBookingChecker(ICurrentDateTimeProvider currentDateTimeProvider, BellaHairContext db)
        {
            _currentDateTimeProvider = currentDateTimeProvider;
            _db = db;
        }

        // Returnerer en bool, der indikerer om kunden med det givne id har bookinger, der ligger i fremtiden
        async Task<bool> IPCustomerFutureBookingChecker.CheckFutureBookings(Guid id)
        {
            return await _db.PrivateCustomers
                .Where(p => p.Id == id)
                .AnyAsync(p => p.Bookings.Any(b => b.EndDateTime > _currentDateTimeProvider.GetCurrentDateTime()));
        }
    }
}
