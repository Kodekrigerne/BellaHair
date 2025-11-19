using BellaHair.Domain;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Infrastructure.PrivateCustomers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Infrastructure.Tests.PrivateCustomers
{
    // Mikkel Dahlmann
    
    internal sealed class PrivateCustomerRepositoryTests : InfrastructureTestBase
    {
        [Test]
        public void Given_NewPrivateCustomer_Then_AddsPrivateCustomerToDatabase()
        {
            // Arrange
            var repo = (IPrivateCustomerRepository)new PrivateCustomerRepository(_db);

            var name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            var birthday = DateTime.Now.AddYears(-19);

            var customer = PrivateCustomerFactory.Create(name, address, phoneNumber, email, birthday);

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
            var repo = (IPrivateCustomerRepository)new PrivateCustomerRepository(_db);

            var name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            var birthday = DateTime.Now.AddYears(-19);

            var customer = PrivateCustomerFactory.Create(name, address, phoneNumber, email, birthday);

            _db.AddAsync(customer);
            _db.SaveChangesAsync();

            // Act
            var returnedCustomer = repo.GetAsync(customer.Id).GetAwaiter().GetResult();

            // Arrange
            Assert.That(returnedCustomer.Id, Is.EqualTo(customer.Id));
        }

        [Test]
        public void Given_PrivateCustomerExistsInDatabase_Then_DeletePrivateCustomerFromDatabase()
        {
            // Arrange
            var repo = (IPrivateCustomerRepository)new PrivateCustomerRepository(_db);

            var name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            var birthday = DateTime.Now.AddYears(-19);

            var customer = PrivateCustomerFactory.Create(name, address, phoneNumber, email, birthday);

            _db.AddAsync(customer);
            _db.SaveChangesAsync();

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
            var repo = (IPrivateCustomerRepository)new PrivateCustomerRepository(_db);
            var invalidId = Guid.NewGuid();

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetAsync(invalidId));
        }
    }
}
