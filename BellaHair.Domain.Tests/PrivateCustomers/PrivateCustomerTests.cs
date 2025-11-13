using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.Tests.PrivateCustomers
{
    internal sealed class PrivateCustomerTests
    {
        [Test]
        public void Given_ValidInputs_Then_CreatesPrivateCustomer()
        {
            // Arrange
            Name name = Name.FromStrings("Mikkel", "Dahlmann", "Frostholm");
            Address address = Address.Create("Vej", "By", "1", 7100, 2);
            PhoneNumber phoneNumber = PhoneNumber.FromString("12345678");
            Email email = Email.FromString("email@email.com");
            DateTime birthday = DateTime.UtcNow;

            // Act
            PrivateCustomer privateCustomer = PrivateCustomer.Create(name, address, phoneNumber, email, birthday);

            // Assert


        }
    }
}
