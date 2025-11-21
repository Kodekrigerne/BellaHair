using BellaHair.Domain.Bookings;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;
using FixtureBuilder;
using Moq;

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

            var booking = Fixture.New<Booking>().With(b => b.Employee, employee).With(b => b.Treatment, treatment).With(b => b.Customer, customer).With(b => b.IsPaid, false).Build();

            //Act
            booking.PayBooking(dateTimeProvider.Object);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(booking.IsPaid, Is.True);
                Assert.That(booking.PaidDateTime, Is.EqualTo(dateTimeProvider.Object.GetCurrentDateTime()));
                Assert.That(booking.EmployeeSnapshot, Is.Not.Null);
                Assert.That(booking.EmployeeSnapshot!.EmployeeId, Is.EqualTo(employee.Id));
                Assert.That(booking.TreatmentSnapshot, Is.Not.Null);
                Assert.That(booking.TreatmentSnapshot!.TreatmentId, Is.EqualTo(treatment.Id));
                Assert.That(booking.CustomerSnapshot, Is.Not.Null);
                Assert.That(booking.CustomerSnapshot!.CustomerId, Is.EqualTo(customer.Id));
            });
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
