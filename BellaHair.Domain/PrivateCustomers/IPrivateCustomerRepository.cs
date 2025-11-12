using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary>
    /// Defines the contract for a repository that manages private customer entities.
    /// </summary>
    
    public interface IPrivateCustomerRepository
    {
        Task AddAsync(PrivateCustomer privateCustomer);
        void Delete(PrivateCustomer privateCustomer);
        Task<PrivateCustomer> Get(Guid id);
        Task SaveChangesAsync();
    }
}
