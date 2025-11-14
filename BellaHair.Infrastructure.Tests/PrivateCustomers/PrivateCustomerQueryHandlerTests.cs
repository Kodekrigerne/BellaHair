using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Infrastructure.PrivateCustomers;
using BellaHair.Ports.PrivateCustomers;

namespace BellaHair.Infrastructure.Tests.PrivateCustomers
{
    internal sealed class PrivateCustomerQueryHandlerTests : InfrastructureTestBase
    {
        [Test]
        public void Given_MultiplePrivateCustomersExists_Then_GetsValidListOfPrivateCustomers()
        {
            // Arrange
            var handler = (IPrivateCustomerQuery)new PrivateCustomerQueryHandler(_db);

            var customer0 = PrivateCustomer.Create(Name.FromStrings("Mikkel", "Dahlmann"),
                Address.Create("Gade", "By", "1", 7100), PhoneNumber.FromString("12345678"),
                Email.FromString("email@email.com"), DateTime.Now.AddYears(-20));

            var customer1 = PrivateCustomer.Create(Name.FromStrings("Mikkel", "Dahlmann"),
                Address.Create("Gade", "By", "1", 7100), PhoneNumber.FromString("12345678"),
                Email.FromString("email@email.com"), DateTime.Now.AddYears(-20));

            _db.AddRange(customer0, customer1);
            _db.SaveChanges();

            // Act
            var privateCustomersList = handler.GetPrivateCustomersAsync().GetAwaiter().GetResult();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(privateCustomersList.Count(), Is.EqualTo(2));
                Assert.That(privateCustomersList.Any(p => p.Id == customer0.Id), Is.True);
                Assert.That(privateCustomersList.Any(p => p.Id == customer1.Id), Is.True);
            });
        }
    }
}
