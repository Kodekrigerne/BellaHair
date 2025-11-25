using BellaHair.Domain.PrivateCustomers;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary><inheritdoc cref="IPrivateCustomerRepository"/> on the DbContext.</summary>

    public class PrivateCustomerRepository : IPrivateCustomerRepository
    {
        private readonly BellaHairContext _db;
        private readonly ICustomerVisitsService _customerVisitsService;

        public PrivateCustomerRepository(BellaHairContext db, ICustomerVisitsService customerVisitsService)
        {
            _db = db;
            _customerVisitsService = customerVisitsService;
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
            var privateCustomer = await _db.PrivateCustomers
                .FirstOrDefaultAsync(p => p.Id == id)
                    ?? throw new KeyNotFoundException($"No private customer exists with ID {id}");

            var visits = await _customerVisitsService.GetCustomerVisitsAsync(privateCustomer.Id);
            privateCustomer.SetVisits(visits);

            return privateCustomer;
        }

        async Task IPrivateCustomerRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
