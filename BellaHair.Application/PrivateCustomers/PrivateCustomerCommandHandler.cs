using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Ports.PrivateCustomers;

namespace BellaHair.Application.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary>
    /// Handles commands related to creating, updating, and deleting private customer entities.
    /// </summary>
    
    public class PrivateCustomerCommandHandler : IPrivateCustomerCommand
    {
        private readonly IPrivateCustomerRepository _privateCustomerRepo;
        private readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        private readonly IPCustomerFutureBookingChecker _pCustomerFutureBookingChecker;

        public PrivateCustomerCommandHandler(IPrivateCustomerRepository privateCustomerRepo, ICurrentDateTimeProvider currentDateTimeProvider, IPCustomerFutureBookingChecker pCustomerFutureBookingChecker)
        {
            _privateCustomerRepo = privateCustomerRepo;
            _currentDateTimeProvider = currentDateTimeProvider;
            _pCustomerFutureBookingChecker = pCustomerFutureBookingChecker;
        }

        async Task IPrivateCustomerCommand.CreatePrivateCustomerAsync(CreatePrivateCustomerCommand command)
        {
            var name = Name.FromStrings(
                command.FirstName,
                command.LastName,
                command.MiddleName);
            
            var address = Address.Create(
                command.streetName,
                command.city,
                command.streetNumber,
                command.zipCode,
                command.floor);
            
            var phoneNumber = PhoneNumber.FromString(command.PhoneNumber);
            var email = Email.FromString(command.Email);

            var customerToCreate = PrivateCustomer.Create(
                name,
                address,
                phoneNumber,
                email,
                command.Birthday,
                _currentDateTimeProvider);

            await _privateCustomerRepo.AddAsync(customerToCreate);
            await _privateCustomerRepo.SaveChangesAsync();
        }

        async Task IPrivateCustomerCommand.DeletePrivateCustomerAsync(DeletePrivateCustomerCommand command)
        {
            var customerToDelete = await _privateCustomerRepo.GetAsync(command.Id);

            if (await _pCustomerFutureBookingChecker.CheckFutureBookings(customerToDelete.Id)) 
                throw new PrivateCustomerException("Customer has future bookings. Delete bookings prior to deleting customer.");

            _privateCustomerRepo.Delete(customerToDelete);

            await _privateCustomerRepo.SaveChangesAsync();
        }

        async Task IPrivateCustomerCommand.UpdatePrivateCustomerAsync(UpdatePrivateCustomerCommand command)
        {
            var customerToUpdate = await _privateCustomerRepo.GetAsync(command.Id);

            var updatedName = Name.FromStrings(
                command.FirstName,
                command.LastName,
                command.MiddleName);
            
            var updatedAddress = Address.Create(
                command.streetName,
                command.city,
                command.streetNumber,
                command.zipCode,
                command.floor);
            
            var updatedPhoneNumber = PhoneNumber.FromString(command.PhoneNumber);
            var updatedEmail = Email.FromString(command.Email);

            customerToUpdate.Update(
                updatedName,
                updatedAddress,
                updatedPhoneNumber,
                updatedEmail,
                command.Birthday,
                _currentDateTimeProvider);

            await _privateCustomerRepo.SaveChangesAsync();
        }
    }
}
