using BellaHair.Domain;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Ports.PrivateCustomers;
using SharedKernel;

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
        private readonly ICustomerOverlapChecker _customerOverlapChecker;

        public PrivateCustomerCommandHandler(
            IPrivateCustomerRepository privateCustomerRepo,
            ICurrentDateTimeProvider currentDateTimeProvider,
            IPCustomerFutureBookingChecker pCustomerFutureBookingChecker,
            ICustomerOverlapChecker customerOverlapChecker)
        {
            _privateCustomerRepo = privateCustomerRepo;
            _currentDateTimeProvider = currentDateTimeProvider;
            _pCustomerFutureBookingChecker = pCustomerFutureBookingChecker;
            _customerOverlapChecker = customerOverlapChecker;
        }

        async Task IPrivateCustomerCommand.CreatePrivateCustomerAsync(CreatePrivateCustomerCommand command)
        {
            await _customerOverlapChecker.OverlapsWithCustomer(command.PhoneNumber, command.Email.ToLowerInvariant());

            var name = Name.FromStrings(
                FormatName(command.FirstName),
                FormatName(command.LastName),
                FormatName(command.MiddleName));

            var address = Address.Create(
                FormatName(command.StreetName),
                FormatName(command.City),
                command.StreetNumber,
                command.ZipCode,
                command.Floor);

            var phoneNumber = PhoneNumber.FromString(command.PhoneNumber);
            var email = Email.FromString(command.Email.ToLowerInvariant());

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

            // Tjekker for fremtidige bookinger tilknyttet kunden der ønskes slettet.
            // Kaster en fejl, hvis kunden har fremtidige bookinger.
            if (await _pCustomerFutureBookingChecker.CheckFutureBookings(customerToDelete.Id))
                throw new PrivateCustomerException("Customer has future bookings. Delete bookings prior to deleting customer.");

            _privateCustomerRepo.Delete(customerToDelete);

            await _privateCustomerRepo.SaveChangesAsync();
        }

        async Task IPrivateCustomerCommand.UpdatePrivateCustomerAsync(UpdatePrivateCustomerCommand command)
        {
            await _customerOverlapChecker.OverlapsWithCustomer(command.PhoneNumber, command.Email.ToLowerInvariant(), command.Id);

            var customerToUpdate = await _privateCustomerRepo.GetAsync(command.Id);

            var updatedName = Name.FromStrings(
                FormatName(command.FirstName),
                FormatName(command.LastName),
                FormatName(command.MiddleName));

            var updatedAddress = Address.Create(
                FormatName(command.StreetName),
                FormatName(command.City),
                command.StreetNumber,
                command.ZipCode,
                command.Floor);

            var updatedPhoneNumber = PhoneNumber.FromString(command.PhoneNumber);
            var updatedEmail = Email.FromString(command.Email.ToLowerInvariant());

            customerToUpdate.Update(
                updatedName,
                updatedAddress,
                updatedPhoneNumber,
                updatedEmail,
                command.Birthday,
                _currentDateTimeProvider);

            await _privateCustomerRepo.SaveChangesAsync();
        }

        private static string FormatName(string? input)
        {
            if (string.IsNullOrEmpty(input)) return input!;

            string firstChar = input[0].ToString().ToUpperInvariant();
            string restOfString = input[1..].ToLowerInvariant();

            return firstChar + restOfString;
        }
    }
}
