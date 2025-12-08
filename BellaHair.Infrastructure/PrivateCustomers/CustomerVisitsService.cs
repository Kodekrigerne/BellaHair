using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace BellaHair.Infrastructure.PrivateCustomers
{
    //Dennis
    /// <inheritdoc cref="ICustomerVisitsService"/>
    public class CustomerVisitsService : ICustomerVisitsService
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public CustomerVisitsService(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        async Task<int> ICustomerVisitsService.GetCustomerVisitsAsync(Guid customerId)
        {
            var now = _currentDateTimeProvider.GetCurrentDateTime();

            return await _db.PrivateCustomers.
                AsNoTracking()
                .Where(c => c.Id == customerId)
                //Skal det være start time eller end time? Tæller besøget du betaler for med her?
                .Select(c => c.Bookings.Count(b => b.StartDateTime < now && b.IsPaid))
                .SingleAsync();
        }
    }
}
