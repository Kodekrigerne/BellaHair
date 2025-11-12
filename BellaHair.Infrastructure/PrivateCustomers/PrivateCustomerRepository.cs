using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.PrivateCustomers;

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

        async Task<PrivateCustomer> IPrivateCustomerRepository.Get(Guid id)
        {
            var privateCustomer = await _db.PrivateCustomers.FindAsync(id)
                                  ?? throw new KeyNotFoundException($"No private customer exists with ID {id}");

            return privateCustomer;
        }

        async Task IPrivateCustomerRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
