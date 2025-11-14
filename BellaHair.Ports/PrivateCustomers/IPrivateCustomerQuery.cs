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
        Task<List<PrivateCustomerDTO>> GetPrivateCustomersAsync();
    }

    public record PrivateCustomerDTO(
        Guid Id,
        string FirstName,
        string? MiddleName,
        string LastName,
        string streetName,
        string city,
        string streetNumber,
        int zipCode,
        int? floor,
        string PhoneNumber,
        string Email,
        DateTime Birthday);
}
