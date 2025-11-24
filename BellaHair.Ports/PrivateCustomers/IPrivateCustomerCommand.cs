using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Ports.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary>
    /// Defines the contract for commands that operate on private customer entities.
    /// </summary>
    
    public interface IPrivateCustomerCommand
    {
        Task CreatePrivateCustomerAsync(CreatePrivateCustomerCommand command);
        Task DeletePrivateCustomerAsync(DeletePrivateCustomerCommand command);
        Task UpdatePrivateCustomerAsync(UpdatePrivateCustomerCommand command);
    }
    
    public record CreatePrivateCustomerCommand(
        string FirstName,
        string? MiddleName,
        string LastName,
        string StreetName, 
        string City, 
        string StreetNumber, 
        int ZipCode, 
        int? Floor,
        string PhoneNumber,
        string Email,
        DateTime Birthday);

    public record UpdatePrivateCustomerCommand(
        Guid Id,
        string FirstName,
        string? MiddleName,
        string LastName,
        string StreetName,
        string City,
        string StreetNumber,
        int ZipCode,
        int? Floor,
        string PhoneNumber,
        string Email,
        DateTime Birthday);

    public record DeletePrivateCustomerCommand(Guid Id);

}
