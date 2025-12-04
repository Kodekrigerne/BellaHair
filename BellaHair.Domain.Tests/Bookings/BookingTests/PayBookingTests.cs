using System.Reflection;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;
using FixtureBuilder;
using Moq;
using NUnit.Framework.Internal;

namespace BellaHair.Domain.Tests.Bookings.BookingTests
{
    internal sealed class PayBookingTests
    {
        [Test]
        public void Given_UnpaidBooking_Then_BookingIsPaid()
        {
            //Arrange
            var customer = Fixture.New<PrivateCustomer>().With(p => p.Id, Guid.NewGuid()).Build();
            var treatment = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();
            var employee = Fixture.New<Employee>().With(e => e.Id, Guid.NewGuid()).WithField("_treatments", [treatment]).Build();
            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            var booking = Fixture.New<Booking>()
                .With(b => b.Employee, employee)
                .With(b => b.Treatment, treatment)
                .With(b => b.Customer, customer)
                .With(b => b.IsPaid, false).Build();

            //Act
            booking.PayBooking(dateTimeProvider.Object);

            //Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(booking.IsPaid, Is.True);
                Assert.That(booking.PaidDateTime, Is.EqualTo(dateTimeProvider.Object.GetCurrentDateTime()));
                Assert.That(booking.EmployeeSnapshot, Is.Not.Null);
                Assert.That(booking.EmployeeSnapshot!.EmployeeId, Is.EqualTo(employee.Id));
                Assert.That(booking.TreatmentSnapshot, Is.Not.Null);
                Assert.That(booking.TreatmentSnapshot!.TreatmentId, Is.EqualTo(treatment.Id));
                Assert.That(booking.CustomerSnapshot, Is.Not.Null);
                Assert.That(booking.CustomerSnapshot!.CustomerId, Is.EqualTo(customer.Id));
            }
        }

        [Test]
        public void Given_UnpaidBooking_Then_ProductsSavedToSnapshots()
        {
            //Arrange
            var name = "Test product name";
            var productLine = Fixture.New<ProductLine>().With(pl => pl.Product.Name, name).Build();

            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            var booking = Fixture.New<Booking>()
                .With(b => b.Employee!.Id, Guid.NewGuid())
                .With(b => b.Treatment!.Id, Guid.NewGuid())
                .With(b => b.Customer!.Id, Guid.NewGuid())
                .With(b => b.ProductLines, [productLine])
                .With(b => b.IsPaid, false).Build();

            //Act
            booking.PayBooking(dateTimeProvider.Object);

            //Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(booking.ProductLineSnapshots, Is.Not.Null);
                Assert.That(booking.ProductLineSnapshots![0].Name, Is.EqualTo("Test product name"));
            }
        }

        [Test]
        public void Given_UnpaidBooking_Then_ProductLinesCleared()
        {
            //Arrange
            var productLine = Fixture.New<ProductLine>().Build();

            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            var booking = Fixture.New<Booking>()
                .With(b => b.Employee!.Id, Guid.NewGuid())
                .With(b => b.Treatment!.Id, Guid.NewGuid())
                .With(b => b.Customer!.Id, Guid.NewGuid())
                .With(b => b.ProductLines, [productLine])
                .With(b => b.IsPaid, false).Build();

            //Act
            booking.PayBooking(dateTimeProvider.Object);

            var productLines = (List<ProductLine>)booking.GetType().GetField("_productLines", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(booking)!;

            //Assert
            Assert.That(productLines, Has.Count.Zero);
        }

        [Test]
        public void Given_PaidBooking_Then_ThrowsException()
        {
            //Arrange
            var customer = Fixture.New<PrivateCustomer>().Build();
            var treatment = Fixture.New<Treatment>().Build();
            var employee = Fixture.New<Employee>().Build();
            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();

            var booking = Fixture.New<Booking>().With(b => b.Employee, employee).With(b => b.Treatment, treatment).With(b => b.Customer, customer).With(b => b.IsPaid, true).Build();

            //Act
            Assert.Throws<BookingException>(() => booking.PayBooking(dateTimeProvider.Object));
        }
    }
}
