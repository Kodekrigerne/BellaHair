using BellaHair.Domain.PrivateCustomers;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary><inheritdoc cref="IPrivateCustomerRepository"/> on the DbContext.</summary>

    public class PrivateCustomerRepository : IPrivateCustomerRepository
    {
        private readonly BellaHairContext _db;
        public PrivateCustomerRepository(BellaHairContext db) => _db = db;

        async Task IPrivateCustomerRepository.AddAsync(PrivateCustomer privateCustomer)
        {
            await _db.PrivateCustomers.AddAsync(privateCustomer);
        }

        void IPrivateCustomerRepository.Delete(PrivateCustomer privateCustomer)
        {
            _db.PrivateCustomers.Remove(privateCustomer);
        }

        // .Include sikrer, at det hentede PrivateCustomer-objekt inkluderer alle booking-relationer
        async Task<PrivateCustomer> IPrivateCustomerRepository.GetAsync(Guid id)
        {
            var privateCustomer = await _db.PrivateCustomers
                                      .Include(p => p.Bookings) //TODO: Fix dette når vi har valgt en Visits strategi
                                      .FirstOrDefaultAsync(p => p.Id == id)
                                  ?? throw new KeyNotFoundException($"No private customer exists with ID {id}");

            return privateCustomer;
        }

        async Task IPrivateCustomerRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
