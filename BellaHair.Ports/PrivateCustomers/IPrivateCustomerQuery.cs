using BellaHair.Domain;
using BellaHair.Domain.SharedValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Ports.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary>
    /// Defines a query interface for retrieving private customer records asynchronously.
    /// </summary>
    
    public interface IPrivateCustomerQuery
    {
        Task<List<PrivateCustomerDTO>> GetPrivateCustomers();
    }

    public record PrivateCustomerDTO(
        Guid Id,
        Name Name,
        Address Address,
        PhoneNumber PhoneNumber,
        Email Email,
        DateTime Birthday);
}
