using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Infrastructure;

namespace BellaHair.Domain.Tests.PrivateCustomers
{
    // Mikkel Dahlmann
    
    internal sealed class PrivateCustomerTests
    {
        [Test]
        public void Given_ValidInputs_Then_CreatesPrivateCustomer()
        {
            // Arrange
            var dateTimeProvider = new CurrentDateTimeProvider() as ICurrentDateTimeProvider;
            var name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            var birthday = dateTimeProvider.GetCurrentDateTime().AddYears(-19);

            // Act
            var privateCustomer = PrivateCustomer.Create(name, address, phoneNumber, email, birthday, dateTimeProvider);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(privateCustomer.Name.FullName, Is.EqualTo(name.FullName));
                Assert.That(privateCustomer.Address.FullAddress, Is.EqualTo(address.FullAddress));
                Assert.That(privateCustomer.PhoneNumber.Value, Is.EqualTo(phoneNumber.Value));
                Assert.That(privateCustomer.Email.Value, Is.EqualTo(email.Value));
                Assert.That(privateCustomer.Birthday, Is.EqualTo(birthday));
            });
        }

        [Test]
        public void Given_ValidInputs_Then_UpdatesPrivateCustomer()
        {
            // Arrange
            var dateTimeProvider = new CurrentDateTimeProvider() as ICurrentDateTimeProvider;
            var name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            var birthday = dateTimeProvider.GetCurrentDateTime().AddYears(-19);

            var privateCustomer = PrivateCustomer.Create(name, address, phoneNumber, email, birthday, dateTimeProvider);

            var newPhoneNumber = PhoneNumber.FromString("87654321");

            // Act
            privateCustomer.Update(name, address, newPhoneNumber, email, birthday, dateTimeProvider);

            // Assert
            Assert.That(privateCustomer.PhoneNumber, Is.EqualTo(newPhoneNumber));
        }

        [Test]
        public void Given_InvalidBirthdayOnCreation_Then_ThrowsPrivateCustomerException()
        {
            // Arrange
            var dateTimeProvider = new CurrentDateTimeProvider() as ICurrentDateTimeProvider;
            var name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            var birthday = dateTimeProvider.GetCurrentDateTime();

            // Act & Assert
            Assert.Throws<PrivateCustomerException>(() => PrivateCustomer.Create(name, address, phoneNumber, email, birthday, dateTimeProvider));
        }

        [Test]
        public void Given_InvalidBirthdayOnUpdate_Then_ThrowsPrivateCustomerException()
        {
            // Arrange
            var dateTimeProvider = new CurrentDateTimeProvider() as ICurrentDateTimeProvider;
            var name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            var birthday = dateTimeProvider.GetCurrentDateTime().AddYears(-19);

            var privateCustomer = PrivateCustomer.Create(name, address, phoneNumber, email, birthday, dateTimeProvider);

            var newBirthday = dateTimeProvider.GetCurrentDateTime();

            // Act & Assert
            Assert.Throws<PrivateCustomerException>(() => privateCustomer.Update(name, address, phoneNumber, email, newBirthday, dateTimeProvider));
        }
    }
}
