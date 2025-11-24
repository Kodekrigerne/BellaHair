using BellaHair.Domain;
using BellaHair.Domain.PrivateCustomers;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary><inheritdoc cref="IPrivateCustomerRepository"/> on the DbContext.</summary>

    public class PrivateCustomerRepository : IPrivateCustomerRepository
    {
        private readonly BellaHairContext _db;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public PrivateCustomerRepository(BellaHairContext db, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            _db = db;
            _currentDateTimeProvider = currentDateTimeProvider;
        }

        async Task IPrivateCustomerRepository.AddAsync(PrivateCustomer privateCustomer)
        {
            await _db.PrivateCustomers.AddAsync(privateCustomer);
        }

        void IPrivateCustomerRepository.Delete(PrivateCustomer privateCustomer)
        {
            _db.PrivateCustomers.Remove(privateCustomer);
        }

        async Task<PrivateCustomer> IPrivateCustomerRepository.GetAsync(Guid id)
        {
            var now = _currentDateTimeProvider.GetCurrentDateTime();

            var privateCustomer = await _db.PrivateCustomers
                .FirstOrDefaultAsync(p => p.Id == id)
                    ?? throw new KeyNotFoundException($"No private customer exists with ID {id}");

            var visits = await _db.PrivateCustomers.AsNoTracking().Where(c => c.Id == id).Select(c => c.Bookings.Count(b => b.EndDateTime < now)).SingleOrDefaultAsync();
            privateCustomer.SetVisits(visits);

            return privateCustomer;
        }

        async Task IPrivateCustomerRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
