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
    /// Defines the contract for commands that operate on private customer entities.
    /// </summary>
    
    public interface IPrivateCustomerCommand
    {
        Task CreatePrivateCustomerAsync(CreatePrivateCustomerCommand command);
        Task DeletePrivateCustomerAsync(DeletePrivateCustomerCommand command);
        Task UpdatePrivateCustomerAsync(UpdatePrivateCustomerCommand command);
    }
    
    public record CreatePrivateCustomerCommand(
        Name Name,
        Address Address,
        PhoneNumber PhoneNumber,
        Email Email,
        DateTime Birthday);

    public record UpdatePrivateCustomerCommand(
        Guid Id, 
        Name Name,
        Address Address,
        PhoneNumber PhoneNumber,
        Email Email,
        DateTime Birthday);

    public record DeletePrivateCustomerCommand(Guid Id);

}
