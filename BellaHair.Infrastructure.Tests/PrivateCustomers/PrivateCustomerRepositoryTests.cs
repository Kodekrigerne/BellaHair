using BellaHair.Domain;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Infrastructure.PrivateCustomers;

namespace BellaHair.Infrastructure.Tests.PrivateCustomers
{
    // Mikkel Dahlmann

    internal sealed class PrivateCustomerRepositoryTests : InfrastructureTestBase
    {
        [Test]
        public void Given_NewPrivateCustomer_Then_AddsPrivateCustomerToDatabase()
        {
            // Arrange
            var repo = (IPrivateCustomerRepository)new PrivateCustomerRepository(_db, new CurrentDateTimeProvider());
            var dateTimeProvider = (ICurrentDateTimeProvider)new CurrentDateTimeProvider();

            var name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            var birthday = dateTimeProvider.GetCurrentDateTime().AddYears(-19);

            var customer = PrivateCustomer.Create(name, address, phoneNumber, email, birthday, dateTimeProvider);

            // Act
            repo.AddAsync(customer);

            // Assert
            _db.SaveChangesAsync();
            var actualCustomer = _db.PrivateCustomers.First();
            Assert.That(actualCustomer.Id, Is.EqualTo(customer.Id));
        }

        [Test]
        public void Given_PrivateCustomerExistsInDatabase_Then_ReturnsPrivateCustomer()
        {
            // Arrange
            var repo = (IPrivateCustomerRepository)new PrivateCustomerRepository(_db, new CurrentDateTimeProvider());
            var dateTimeProvider = (ICurrentDateTimeProvider)new CurrentDateTimeProvider();


            var name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            var birthday = dateTimeProvider.GetCurrentDateTime().AddYears(-19);

            var customer = PrivateCustomer.Create(name, address, phoneNumber, email, birthday, dateTimeProvider);

            _db.Add(customer);
            _db.SaveChanges();

            // Act
            var returnedCustomer = repo.GetAsync(customer.Id).GetAwaiter().GetResult();

            // Arrange
            Assert.That(returnedCustomer.Id, Is.EqualTo(customer.Id));
        }

        [Test]
        public void Given_PrivateCustomerExistsInDatabase_Then_DeletePrivateCustomerFromDatabase()
        {
            // Arrange
            var repo = (IPrivateCustomerRepository)new PrivateCustomerRepository(_db, new CurrentDateTimeProvider());
            var dateTimeProvider = (ICurrentDateTimeProvider)new CurrentDateTimeProvider();

            var name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            var birthday = dateTimeProvider.GetCurrentDateTime().AddYears(-19);

            var customer = PrivateCustomer.Create(name, address, phoneNumber, email, birthday, dateTimeProvider);

            _db.Add(customer);
            _db.SaveChanges();

            var customerToDelete = _db.PrivateCustomers.First();

            // Act
            repo.Delete(customerToDelete);
            repo.SaveChangesAsync();

            // Assert
            Assert.That(_db.PrivateCustomers.Find(customerToDelete.Id), Is.Null);
        }

        [Test]
        public void Given_PrivateCustomerDoesNotExists_Then_ThrowsPrivateCustomerException()
        {
            // Arrange
            var repo = (IPrivateCustomerRepository)new PrivateCustomerRepository(_db, new CurrentDateTimeProvider());
            var invalidId = Guid.NewGuid();

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetAsync(invalidId));
        }
    }
}
