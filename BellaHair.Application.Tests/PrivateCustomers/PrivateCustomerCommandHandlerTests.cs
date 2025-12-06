using BellaHair.Domain;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Infrastructure;
using BellaHair.Ports.PrivateCustomers;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace BellaHair.Application.Tests.PrivateCustomers
{
    // Mikkel Dahlmann

    internal sealed class PrivateCustomerCommandHandlerTests : ApplicationTestBase
    {
        [Test]
        public void Given_PrivateCustomerData_Then_CreatesPrivateCustomerInDatabase()
        {
            // Arrange

            // Castes til IPrivateCustomerRepository, fordi metodekaldene på repo er eksplicitte.
            var dateTimeProvider = (ICurrentDateTimeProvider)new CurrentDateTimeProvider();
            var handler = ServiceProvider.GetRequiredService<IPrivateCustomerCommand>();
            var command = new CreatePrivateCustomerCommand("Mikkel", null, "Dahlmann",
                "Gade", "By", "1", 7100, null, "12345678", "email@email.com", dateTimeProvider.GetCurrentDateTime().AddYears(-20));

            // Act
            handler.CreatePrivateCustomerAsync(command).GetAwaiter().GetResult();

            // Assert
            var customerFromDb = _db.PrivateCustomers.FirstOrDefault();
            using (Assert.EnterMultipleScope())
            {
                Assert.That(customerFromDb!.Name.FirstName, Is.EqualTo(command.FirstName));
                Assert.That(customerFromDb!.PhoneNumber.Value, Is.EqualTo(command.PhoneNumber));
                Assert.That(customerFromDb!.Birthday, Is.EqualTo(command.Birthday));
            }
        }

        [Test]
        public void Given_PrivateCustomerData_Then_DeletesPrivateCustomerFromDatabase()
        {
            // Arrange
            var dateTimeProvider = (ICurrentDateTimeProvider)new CurrentDateTimeProvider();
            var handler = ServiceProvider.GetRequiredService<IPrivateCustomerCommand>();

            var customer0 = PrivateCustomer.Create(Name.FromStrings("Mikkel", "Dahlmann"),
                Address.Create("Gade", "By", "1", 7100), PhoneNumber.FromString("12345678"),
                Email.FromString("email@email.com"), dateTimeProvider.GetCurrentDateTime().AddYears(-20), dateTimeProvider);

            _db.Add(customer0);
            _db.SaveChanges();

            var command = new DeletePrivateCustomerCommand(customer0.Id);

            //Act
            handler.DeletePrivateCustomerAsync(command);

            //Assert
            Assert.That(_db.PrivateCustomers.Any(), Is.False);
        }

        [Test]
        public void Given_UpdatedPrivateCustomerData_Then_UpdatesPrivateCustomerInDatabase()
        {
            // Arrange
            var dateTimeProvider = (ICurrentDateTimeProvider)new CurrentDateTimeProvider();
            var handler = ServiceProvider.GetRequiredService<IPrivateCustomerCommand>();

            //var handler = (IPrivateCustomerCommand)new PrivateCustomerCommandHandler(repo, dateTimeProvider, bookingChecker);

            var customer0 = PrivateCustomer.Create(Name.FromStrings("Mikkel", "Dahlmann"),
                Address.Create("Gade", "By", "1", 7100), PhoneNumber.FromString("12345678"),
                Email.FromString("email@email.com"), dateTimeProvider.GetCurrentDateTime().AddYears(-20), dateTimeProvider);

            _db.Add(customer0);
            _db.SaveChanges();

            var command = new UpdatePrivateCustomerCommand(customer0.Id, "Mikkel", null, "Dahlmann",
                "Gade", "By", "1", 7100, null, "12345678", "updated@email.com", DateTime.Now.AddYears(-20));

            // Act
            handler.UpdatePrivateCustomerAsync(command);

            // Assert
            Assert.That(_db.PrivateCustomers.FirstOrDefault()!.Email.Value, Is.EqualTo(command.Email));
        }
    }
}
