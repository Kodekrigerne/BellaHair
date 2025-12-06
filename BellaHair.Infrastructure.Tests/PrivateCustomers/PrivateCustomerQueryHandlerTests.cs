using BellaHair.Domain;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Infrastructure.PrivateCustomers;
using BellaHair.Ports.PrivateCustomers;

namespace BellaHair.Infrastructure.Tests.PrivateCustomers
{
    // Mikkel Dahlmann

    internal sealed class PrivateCustomerQueryHandlerTests : InfrastructureTestBase
    {
        [Test]
        public void Given_MultiplePrivateCustomersExists_Then_GetsValidListOfPrivateCustomers()
        {
            // Arrange
            var dateTimeProvider = (ICurrentDateTimeProvider)new CurrentDateTimeProvider();
            var handler = (IPrivateCustomerQuery)new PrivateCustomerQueryHandler(_db, dateTimeProvider, new CustomerVisitsService(_db, dateTimeProvider));

            var customer0 = PrivateCustomer.Create(Name.FromStrings("Mikkel", "Dahlmann"),
                Address.Create("Gade", "By", "1", 7100), PhoneNumber.FromString("12345678"),
                Email.FromString("email@email.com"), dateTimeProvider.GetCurrentDateTime().AddYears(-20), dateTimeProvider);

            var customer1 = PrivateCustomer.Create(Name.FromStrings("Mikkel", "Dahlmann"),
                Address.Create("Gade", "By", "1", 7100), PhoneNumber.FromString("12345678"),
                Email.FromString("email@email.com"), dateTimeProvider.GetCurrentDateTime().AddYears(-20), dateTimeProvider);

            _db.AddRange(customer0, customer1);
            _db.SaveChanges();

            // Act
            var privateCustomersList = handler.GetPrivateCustomersAsync().GetAwaiter().GetResult();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(privateCustomersList, Has.Count.EqualTo(2));
                Assert.That(privateCustomersList.Any(p => p.Id == customer0.Id), Is.True);
                Assert.That(privateCustomersList.Any(p => p.Id == customer1.Id), Is.True);
            }
        }
    }
}
