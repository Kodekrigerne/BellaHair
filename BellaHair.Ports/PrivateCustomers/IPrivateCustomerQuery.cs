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
        string FullName,
        string StreetName,
        string City,
        string StreetNumber,
        int ZipCode,
        int? Floor,
        string FullAddress,
        string PhoneNumber,
        string Email,
        DateTime Birthday,
        int Visists);
}
