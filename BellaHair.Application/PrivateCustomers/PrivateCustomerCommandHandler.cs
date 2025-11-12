using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Ports.PrivateCustomers;

namespace BellaHair.Application.PrivateCustomers
{
    public class PrivateCustomerCommandHandler : IPrivateCustomerCommand
    {
        private readonly IPrivateCustomerRepository _privateCustomerRepo;

        public PrivateCustomerCommandHandler(IPrivateCustomerRepository privateCustomerRepo) =>
            _privateCustomerRepo = privateCustomerRepo;

        async Task IPrivateCustomerCommand.CreatePrivateCustomerAsync(CreatePrivateCustomerCommand command)
        {
            var customerToCreate = PrivateCustomer.Create(command.Name, command.Address, command.PhoneNumber, command.Email,
                command.Birthday);

            await _privateCustomerRepo.AddAsync(customerToCreate);
            await _privateCustomerRepo.SaveChangesAsync();
        }

        async Task IPrivateCustomerCommand.DeletePrivateCustomerAsync(DeletePrivateCustomerCommand command)
        {
            var customerToDelete = await _privateCustomerRepo.GetAsync(command.Id);

            _privateCustomerRepo.Delete(customerToDelete);

            await _privateCustomerRepo.SaveChangesAsync();
        }

        async Task IPrivateCustomerCommand.UpdatePrivateCustomerAsync(UpdatePrivateCustomerCommand command)
        {
            var customerToUpdate = await _privateCustomerRepo.GetAsync(command.Id);

            customerToUpdate.Update(command.Name, command.Address, command.PhoneNumber, command.Email,
                command.Birthday);

            await _privateCustomerRepo.SaveChangesAsync();
        }
    }
}
